using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using NHibernate;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class GetSummonerCommandHandler : ICommandHandler<GetSummonerCommand, Summoner>
    {
        private readonly ISession _session;

        public GetSummonerCommandHandler(ISession session)
        {
            Guard.NotNull(session, nameof(session));

            this._session = session;
        }

        public Task<Summoner> Execute(GetSummonerCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            var summoner = this._session.Get<Summoner>(command.SummonerId);
            return Task.FromResult(summoner);
        }
    }
}