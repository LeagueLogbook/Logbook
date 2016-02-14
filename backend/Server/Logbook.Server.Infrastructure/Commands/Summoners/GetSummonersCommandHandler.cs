using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Entities.Summoners;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class GetSummonersCommandHandler : ICommandHandler<GetSummonersCommand, IList<Summoner>>
    {
        private readonly ISession _session;

        public GetSummonersCommandHandler(ISession session)
        {
            Guard.NotNull(session, nameof(session));

            this._session = session;
        }

        public Task<IList<Summoner>> Execute(GetSummonersCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            var user = this._session.Get<User>(command.UserId);

            if (user == null)
                throw new UserNotFoundException();

            return Task.FromResult(user.WatchSummoners);
        }
    }
}