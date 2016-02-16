using System;
using System.Collections.Generic;
using System.IO.Pipes;
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
using Logbook.Shared;

namespace Logbook.Worker.UpdateSummoners
{
    public class UpdateSummonersWorker : IWorker
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IUpdateSummonerQueue _updateSummonerQueue;

        public UpdateSummonersWorker(ICommandExecutor commandExecutor, IUpdateSummonerQueue updateSummonerQueue)
        {
            Guard.NotNull(commandExecutor, nameof(commandExecutor));
            Guard.NotNull(updateSummonerQueue, nameof(updateSummonerQueue));

            this._commandExecutor = commandExecutor;
            this._updateSummonerQueue = updateSummonerQueue;
        }

        public Task StartAsync()
        {
            return Task.CompletedTask;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                var summonerToUpdate = await this._updateSummonerQueue.TryDequeueSummonerAsync();
                if (summonerToUpdate != null)
                {
                    try
                    {
                        AppInsights.GenerateAsyncAwareOperationId();

                        AppInsights.Client.TrackEvent("Updating Summoner");

                        await this._commandExecutor.Execute(new UpdateSummonerCommand(summonerToUpdate.Value));
                        await this._updateSummonerQueue.RemoveAsync(summonerToUpdate.Value);

                        await this._updateSummonerQueue.EnqueueSummonerAsync(summonerToUpdate.Value);
                    }
                    catch (Exception exception)
                    {
                        await this._updateSummonerQueue.TryAgainLaterAsync(summonerToUpdate.Value);

                        var payload = new Dictionary<string, string>
                        {
                            ["Source"] = "Update Summoners Worker"
                        };
                        AppInsights.Client.TrackException(exception, payload);
                    }
                }
            }
        }
    }
}
