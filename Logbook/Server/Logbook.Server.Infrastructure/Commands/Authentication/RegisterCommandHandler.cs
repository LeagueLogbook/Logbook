using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Localization.Server;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Logbook.Shared.Results;
using Raven.Client;
using Raven.Client.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, User>
    {
        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        private readonly ISecretGenerator _secretGenerator;
        private readonly ISaltCombiner _saltCombiner;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="secretGenerator">The secret generator.</param>
        /// <param name="saltCombiner">The salt combiner.</param>
        public RegisterCommandHandler([NotNull]IAsyncDocumentSession documentSession, [NotNull]ISecretGenerator secretGenerator, [NotNull]ISaltCombiner saltCombiner)
        {
            Guard.AgainstNullArgument(nameof(documentSession), documentSession);
            Guard.AgainstNullArgument(nameof(secretGenerator), secretGenerator);
            Guard.AgainstNullArgument(nameof(saltCombiner), saltCombiner);

            this._documentSession = documentSession;
            this._secretGenerator = secretGenerator;
            this._saltCombiner = saltCombiner;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified <paramref name="command" />.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public async Task<Result<User>> Execute(RegisterCommand command, ICommandScope scope)
        {
            var emailAddressAlreadyInUse = await this._documentSession
                .Query<User, Users_ByEmailAddress>()
                .Where(f => f.EmailAddress == command.EmailAddress)
                .AnyAsync()
                .WithCurrentCulture();

            if (emailAddressAlreadyInUse)
                return Result.AsError(CommandMessages.EmailIsNotAvailable);

            var user = new User
            {
                EmailAddress = command.EmailAddress,
                PreferredLanguage = command.PreferredLanguage
            };

            await this._documentSession.StoreAsync(user).WithCurrentCulture();

            var salt = this._secretGenerator.Generate();
            var authenticationData = new AuthenticationData
            {
                ForUserId = user.Id,
                Authentications =
                {
                    new LogbookAuthenticationKind
                    {
                        Salt = salt,
                        IterationCount = Config.IterationCountForPasswordHashing,
                        Hash = this._saltCombiner.Combine(salt, Config.IterationCountForPasswordHashing, command.PasswordSHA256Hash)
                    }
                }
            };

            await this._documentSession.StoreAsync(authenticationData).WithCurrentCulture();

            return Result.AsSuccess(user);
        }
        #endregion
    }
}