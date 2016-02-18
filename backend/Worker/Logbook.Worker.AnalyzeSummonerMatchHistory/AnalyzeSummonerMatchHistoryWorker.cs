using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Logbook.Server.Contracts;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Contracts.Riot;
using Logbook.Server.Infrastructure;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Entities.Summoners;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Worker.AnalyzeSummonerMatchHistory
{
    public class AnalyzeSummonerMatchHistoryWorker : IWorker
    {
        private readonly IAnalyzeSummonerMatchHistoryQueue _queue;
        private readonly ILeagueService _leagueService;
        private readonly ISessionFactory _sessionFactory;
        private readonly ICommandExecutor _commandExecutor;

        public AnalyzeSummonerMatchHistoryWorker(IAnalyzeSummonerMatchHistoryQueue queue, ILeagueService leagueService, ISessionFactory sessionFactory, ICommandExecutor commandExecutor)
        {
            this._queue = queue;
            this._leagueService = leagueService;
            this._sessionFactory = sessionFactory;
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

                    await this.AnalyzeSummonerMatchHistory(summonerToAnalyze);

                    await this._queue.RemoveAsync(summonerToAnalyze.Value);
                    await this._queue.EnqueueSummonerAsync(summonerToAnalyze.Value);
                }
            }
        }

        private async Task AnalyzeSummonerMatchHistory(int? summonerToAnalyze)
        {
            using (var session = this._sessionFactory.OpenSession())
            {
                var summoner = session.Load<Summoner>(summonerToAnalyze);

                var matchIdHistory = await this._leagueService.GetMatchHistory(summoner.Region, summoner.RiotSummonerId, summoner.LatestAnalyzedMatchTimeStamp);

                foreach (var matchId in matchIdHistory)
                {
                    var match = await this._leagueService.GetMatch(summoner.Region, matchId);

                    if (match.CreationDate > summoner.LatestAnalyzedMatchTimeStamp)
                        summoner.LatestAnalyzedMatchTimeStamp = match.CreationDate;

                    var summonerIds = match.BlueTeam.Participants
                        .Select(f => f.SummonerId)
                        .Concat(match.PurpleTeam.Participants.Select(f => f.SummonerId))
                        .ToList();

                    var existingSummoners = session.Query<Summoner>()
                        .Where(f => f.Region == summoner.Region && summonerIds.Contains(f.RiotSummonerId))
                        .ToList();

                    foreach (var nonExistingSummonerId in summonerIds.Where(f => existingSummoners.Any(d => d.RiotSummonerId == f) == false))
                    {
                        var nonExistingSummoner = await this._leagueService.GetSummonerAsync(summoner.Region, nonExistingSummonerId);
                        await this._commandExecutor.Execute(new AddNewSummonerCommand(nonExistingSummoner));
                    }
                }
            }
        }
    }
}
