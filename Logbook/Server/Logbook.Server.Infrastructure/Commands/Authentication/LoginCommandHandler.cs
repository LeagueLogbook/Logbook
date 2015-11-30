using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Localization.Server;
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
        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        private readonly ISaltCombiner _saltCombiner;
        private readonly IJsonWebTokenService _jsonWebTokenService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="saltCombiner">The salt combiner.</param>
        /// <param name="jsonWebTokenService">The json web token service.</param>
        public LoginCommandHandler([NotNull]IAsyncDocumentSession documentSession, [NotNull] ISaltCombiner saltCombiner, [NotNull]IJsonWebTokenService jsonWebTokenService)
        {
            Guard.AgainstNullArgument(nameof(documentSession), documentSession);
            Guard.AgainstNullArgument(nameof(saltCombiner), saltCombiner);
            Guard.AgainstNullArgument(nameof(jsonWebTokenService), jsonWebTokenService);

            this._documentSession = documentSession;
            this._saltCombiner = saltCombiner;
            this._jsonWebTokenService = jsonWebTokenService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public async Task<Result<string>> Execute(LoginCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument(nameof(command), command);
            Guard.AgainstNullArgument(nameof(scope), scope);

            var user = await this._documentSession.Query<User, Users_ByEmailAddress>()
                .Where(f => f.EmailAddress == command.EmailAddress)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            if (user == null)
                return Result.AsError(string.Format(CommandMessages.NoUserFound, command.EmailAddress));

            var authenticationData = await this._documentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
                .WithCurrentCulture();

            var authentication = authenticationData.Authentications.OfType<LogbookAuthenticationKind>().FirstOrDefault();

            if (authentication == null)
                return Result.AsError(string.Format(CommandMessages.CannotLoginWithPassword, command.EmailAddress));

            var computedHash = this._saltCombiner.Combine(authentication.Salt, authentication.IterationCount, command.PasswordSHA256Hash);

            if (computedHash.SequenceEqual(authentication.Hash) == false)
                return Result.AsError(CommandMessages.IncorrectPassword);

            var token = this._jsonWebTokenService.Generate(user.Id);
            return Result.AsSuccess(token);
        }
        #endregion
    }
}