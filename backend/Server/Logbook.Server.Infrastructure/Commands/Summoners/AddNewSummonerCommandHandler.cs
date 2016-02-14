using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Contracts.Riot;
using Logbook.Shared;
using NHibernate;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class AddNewSummonerCommandHandler : ICommandHandler<AddNewSummonerCommand, object>
    {
        private readonly ISession _session;
        private readonly IUpdateSummonerQueue _updateSummonerQueue;

        public AddNewSummonerCommandHandler(ISession session, IUpdateSummonerQueue updateSummonerQueue)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(updateSummonerQueue, nameof(updateSummonerQueue));

            this._session = session;
            this._updateSummonerQueue = updateSummonerQueue;
        }

        public async Task<object> Execute(AddNewSummonerCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            this._session.SaveOrUpdate(command.Summoner);
            await this._updateSummonerQueue.EnqueueSummonerAsync(command.Summoner.Id);

            return new object();
        }
    }
}