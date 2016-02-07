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
    public class GetUserCommandHandler : ICommandHandler<GetUserCommand, UserModel>
    {
        private readonly ISession _session;
        private readonly IMapper<User, UserModel> _userMapper;

        public GetUserCommandHandler([NotNull]ISession session, [NotNull]IMapper<User, UserModel> userMapper)
        {
            Guard.AgainstNullArgument(nameof(session), session);
            Guard.AgainstNullArgument(nameof(userMapper), userMapper);

            this._session = session;
            this._userMapper = userMapper;
        }

        public async Task<UserModel> Execute([NotNull]GetUserCommand command, [NotNull]ICommandScope scope)
        {
            Guard.AgainstNullArgument(nameof(command), command);
            Guard.AgainstNullArgument(nameof(scope), scope);

            var user = this._session.Get<User>(command.UserId);
            var model = await this._userMapper.MapAsync(user);

            return model;
        }
    }
}