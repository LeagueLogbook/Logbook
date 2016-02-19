using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using NHibernate;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class UpdateAnalyzedMatchHistoryCommandHandler : ICommandHandler<UpdateAnalyzedMatchHistoryCommand, object>
    {
        private readonly ISession _session;

        public UpdateAnalyzedMatchHistoryCommandHandler(ISession session)
        {
            Guard.NotNull(session, nameof(session));

            this._session = session;
        }

        public Task<object> Execute(UpdateAnalyzedMatchHistoryCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            var summoner = this._session.Get<Summoner>(command.SummonerId);

            summoner.LatestAnalyzedMatchTimeStamp = command.LatestAnalyzedMatchTimeStamp;
            summoner.AnalyzedMatchHistory = command.AnalyzedMatchHistory;

            return Task.FromResult(new object());
        }
    }
}