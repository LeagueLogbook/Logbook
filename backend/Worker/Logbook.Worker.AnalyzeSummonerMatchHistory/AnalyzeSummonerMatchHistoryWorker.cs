using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Logbook.Server.Contracts;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Contracts.Riot;
using Logbook.Server.Infrastructure;
using Logbook.Server.Infrastructure.Configuration;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Worker.AnalyzeSummonerMatchHistory
{
    public class AnalyzeSummonerMatchHistoryWorker : IWorker
    {
        private readonly IAnalyzeSummonerMatchHistoryQueue _queue;
        private readonly ILeagueService _leagueService;
        private readonly ICommandExecutor _commandExecutor;

        public AnalyzeSummonerMatchHistoryWorker(IAnalyzeSummonerMatchHistoryQueue queue, ILeagueService leagueService, ICommandExecutor commandExecutor)
        {
            Guard.NotNull(queue, nameof(queue));
            Guard.NotNull(leagueService, nameof(leagueService));
            Guard.NotNull(commandExecutor, nameof(commandExecutor));

            this._queue = queue;
            this._leagueService = leagueService;
            this._commandExecutor = commandExecutor;
        }

        public Task StartAsync()
        {
            return Task.CompletedTask;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                var summonerToAnalyze = await this._queue.TryDequeueSummonerAsync();
                if (summonerToAnalyze != null)
                {
                    AppInsights.GenerateAsyncAwareOperationId();

                    AppInsights.Client.TrackEvent("Analyzing Summoner Match History");

                    await this.AnalyzeSummonerMatchHistory(summonerToAnalyze.Value);
                    
                    await this._queue.RemoveAsync(summonerToAnalyze.Value);
                    await this._queue.EnqueueSummonerAsync(summonerToAnalyze.Value);
                }
            }
        }

        private async Task AnalyzeSummonerMatchHistory(int summonerToAnalyze)
        {
            try
            {
                AppInsights.Client.TrackEvent($"Analyzing match history of summoner {summonerToAnalyze}");

                Stopwatch watch = null;

                var summoner = await this._commandExecutor.Execute(new GetSummonerCommand(summonerToAnalyze));

                var matchIdHistory = await this._leagueService.GetMatchHistory(summoner.Region, summoner.RiotSummonerId, summoner.LatestAnalyzedMatchTimeStamp);

                foreach (var matchId in matchIdHistory)
                {
                    if (watch == null ||
                        watch.Elapsed > TimeSpan.FromMinutes(Config.Riot.RequestMoreTimeToAnalyzeMatchHistoryInMinutes))
                    {
                        await this._queue.RequestMoreTimeToProcess(summonerToAnalyze, TimeSpan.FromMinutes(Config.Riot.RequestMoreTimeToAnalyzeMatchHistoryInMinutes));
                        watch = Stopwatch.StartNew();
                    }

                    AppInsights.Client.TrackEvent($"Analyzing match {matchId}");

                    var match = await this._leagueService.GetMatch(summoner.Region, matchId);
                    await this._commandExecutor.Execute(new UpdateAnalyzedMatchHistoryCommand(summonerToAnalyze, match.CreationDate, new AnalyzedMatchHistory()));
                }
            }
            catch (Exception exception)
            {
                AppInsights.Client.TrackException(exception);
            }
            finally
            {
                AppInsights.Client.TrackEvent($"Finished anlyzing match history of summoner {summonerToAnalyze}");
            }
        }
    }
}
