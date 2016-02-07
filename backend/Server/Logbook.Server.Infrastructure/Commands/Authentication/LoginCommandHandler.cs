using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Localization.Server;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Logbook.Shared.Models;
using Logbook.Shared.Models.Authentication;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, JsonWebToken>
    {
        #region Fields
        private readonly ISession _session;
        private readonly ISaltCombiner _saltCombiner;
        private readonly IJsonWebTokenService _jsonWebTokenService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
        /// </summary>
        /// <param name="session">The database session.</param>
        /// <param name="saltCombiner">The salt combiner.</param>
        /// <param name="jsonWebTokenService">The json web token service.</param>
        public LoginCommandHandler([NotNull]ISession session, [NotNull] ISaltCombiner saltCombiner, [NotNull]IJsonWebTokenService jsonWebTokenService)
        {
            Guard.AgainstNullArgument(nameof(session), session);
            Guard.AgainstNullArgument(nameof(saltCombiner), saltCombiner);
            Guard.AgainstNullArgument(nameof(jsonWebTokenService), jsonWebTokenService);

            this._session = session;
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
        public async Task<JsonWebToken> Execute(LoginCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument(nameof(command), command);
            Guard.AgainstNullArgument(nameof(scope), scope);

            var user = this._session.Query<User>()
                .Where(f => f.EmailAddress.ToUpper() == command.EmailAddress.Trim().ToUpper())
                .FetchMany(f => f.Authentications)
                .FirstOrDefault();
            
            if (user == null)
                throw new UserNotFoundException();
            
            var authentication = user.Authentications.OfType<LogbookAuthenticationKind>().FirstOrDefault();

            if (authentication == null)
                throw new CannotLoginWithPasswordException();

            var computedHash = this._saltCombiner.Combine(authentication.Salt, authentication.IterationCount, command.PasswordSHA256Hash);

            if (computedHash.SequenceEqual(authentication.Hash) == false)
                throw new IncorrectPasswordException();

            return this._jsonWebTokenService.GenerateForLogin(user.Id);
        }
        #endregion
    }
}