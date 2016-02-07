using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Users;
using Logbook.Server.Contracts.Mapping;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Models.Authentication;
using NHibernate;

namespace Logbook.Server.Infrastructure.Commands.Users
{
    public class GetUserCommandHandler : ICommandHandler<GetUserCommand, User>
    {
        private readonly ISession _session;

        public GetUserCommandHandler([NotNull]ISession session)
        {
            Guard.AgainstNullArgument(nameof(session), session);

            this._session = session;
        }

        public Task<User> Execute([NotNull]GetUserCommand command, [NotNull]ICommandScope scope)
        {
            Guard.AgainstNullArgument(nameof(command), command);
            Guard.AgainstNullArgument(nameof(scope), scope);

            var user = this._session.Get<User>(command.UserId);
            return Task.FromResult(user);
        }
    }
}