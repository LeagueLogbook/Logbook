using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Users;
using Logbook.Server.Contracts.Mapping;
using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Models.Authentication;
using NHibernate;

namespace Logbook.Server.Infrastructure.Commands.Users
{
    public class GetUserCommandHandler : ICommandHandler<GetUserCommand, User>
    {
        private readonly ISession _session;

        public GetUserCommandHandler(ISession session)
        {
            Guard.NotNull(session, nameof(session));

            this._session = session;
        }

        public Task<User> Execute(GetUserCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            var user = this._session.Get<User>(command.UserId);
            return Task.FromResult(user);
        }
    }
}