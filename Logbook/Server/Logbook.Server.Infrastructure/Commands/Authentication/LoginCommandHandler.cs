using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Logbook.Shared.Results;
using Raven.Client;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, string>
    {
        private readonly IAsyncDocumentSession _documentSession;
        private readonly ISaltCombiner _saltCombiner;
        private readonly IJsonWebTokenService _jsonWebTokenService;

        public LoginCommandHandler([NotNull]IAsyncDocumentSession documentSession, [NotNull] ISaltCombiner saltCombiner, [NotNull]IJsonWebTokenService jsonWebTokenService)
        {
            Guard.AgainstNullArgument(nameof(documentSession), documentSession);
            Guard.AgainstNullArgument(nameof(saltCombiner), saltCombiner);
            Guard.AgainstNullArgument(nameof(jsonWebTokenService), jsonWebTokenService);

            this._documentSession = documentSession;
            this._saltCombiner = saltCombiner;
            this._jsonWebTokenService = jsonWebTokenService;
        }

        public async Task<Result<string>> Execute(LoginCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument(nameof(command), command);
            Guard.AgainstNullArgument(nameof(scope), scope);

            var user = await this._documentSession.Query<User, Users_ByEmailAddress>()
                .Where(f => f.EmailAddress == command.EmailAddress)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            if (user == null)
                return Result.AsError("No user found");

            var authenticationData = await this._documentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
                .WithCurrentCulture();

            var computedHash = this._saltCombiner.Combine(authenticationData.Salt, authenticationData.IterationCount, command.PasswordSHA256Hash);

            if (computedHash.SequenceEqual(authenticationData.Hash) == false)
                return Result.AsError("Incorrect password");

            var token = this._jsonWebTokenService.Generate(user.Id);
            return Result.AsSuccess(token);
        }
    }
}