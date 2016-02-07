using System;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Entities.Summoners;
using NHibernate;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class RemoveSummonerCommandHandler : ICommandHandler<RemoveSummonerCommand, object>
    {
        private readonly ISession _session;

        public RemoveSummonerCommandHandler(ISession session)
        {
            this._session = session;
        }

        public Task<object> Execute(RemoveSummonerCommand command, ICommandScope scope)
        {
            var user = this._session.Get<User>(command.UserId);

            if (user == null)
                throw new UserNotFoundException();

            var summonerToRemove = user.WatchSummoners.FirstOrDefault(f => f.RiotSummonerId == command.SummonerId && f.Region == command.Region);
            user.WatchSummoners.Remove(summonerToRemove);

            return Task.FromResult(new object());
        }
    }
}