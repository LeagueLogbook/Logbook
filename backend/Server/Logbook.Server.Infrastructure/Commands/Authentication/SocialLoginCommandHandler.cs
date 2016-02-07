using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Logbook.Shared.Models.Authentication;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public abstract class SocialLoginCommandHandler<TCommand> : ICommandHandler<TCommand, JsonWebToken>
        where TCommand : ICommand<JsonWebToken>
    {
        #region Fields
        private readonly ISession _session;
        private readonly IJsonWebTokenService _jsonWebTokenService;
        #endregion

        #region Constructors
        protected SocialLoginCommandHandler(ISession session, IJsonWebTokenService jsonWebTokenService)
        {
            Guard.AgainstNullArgument(nameof(session), session);
            Guard.AgainstNullArgument(nameof(jsonWebTokenService), jsonWebTokenService);

            this._session = session;
            this._jsonWebTokenService = jsonWebTokenService;
        }
        #endregion

        #region Methods
        public async Task<JsonWebToken> Execute(TCommand command, ICommandScope scope)
        {
            var socialLoginUser = await this.GetMeAsync(command).WithCurrentCulture();

            if (socialLoginUser == null)
                throw new InternalServerErrorException();

            IList<Func<SocialLoginUser, Task<int?>>> casesToCheck = new List<Func<SocialLoginUser, Task<int?>>>
            {
                this.FindUserIdBySocialLoginUserId,
                this.FindUserIdByEmailAddress,
                this.CreateNewUser
            };

            foreach (var caseToCheck in casesToCheck)
            {
                var userId = await caseToCheck(socialLoginUser).WithCurrentCulture();
                if (userId != null)
                {
                    return this._jsonWebTokenService.GenerateForLogin(userId.Value);
                }
            }

            throw new InternalServerErrorException();
        }
        #endregion

        #region Private Methods
        private Task<int?> FindUserIdBySocialLoginUserId(SocialLoginUser socialLoginUser)
        {
            var user = this.GetUserForSocialUser(socialLoginUser);
            return Task.FromResult(user?.Id);
        }

        private Task<int?> FindUserIdByEmailAddress(SocialLoginUser socialLoginUser)
        {
            var user = this._session.Query<User>()
                .FetchMany(f => f.Authentications)
                .FirstOrDefault(f => f.EmailAddress.ToUpper() == socialLoginUser.EmailAddress.Trim().ToUpper());

            if (user == null)
                return Task.FromResult((int?)null);

            var authentication = this.CreateAuthentication(socialLoginUser);
            user.Authentications.Add(authentication);
            authentication.User = user;

            return Task.FromResult((int?)user.Id);
        }

        private Task<int?> CreateNewUser(SocialLoginUser socialLoginUser)
        {
            var authentication = this.CreateAuthentication(socialLoginUser);

            var user = new User
            {
                EmailAddress = socialLoginUser.EmailAddress,
                PreferredLanguage = new CultureInfo(socialLoginUser.Locale).Parent.TwoLetterISOLanguageName,
                Authentications =
                {
                    authentication
                }
            };
            authentication.User = user;

            this._session.SaveOrUpdate(user);

            return Task.FromResult((int?)user.Id);
        }

        protected abstract Task<SocialLoginUser> GetMeAsync(TCommand command);

        protected abstract User GetUserForSocialUser(SocialLoginUser user);

        protected abstract AuthenticationKindBase CreateAuthentication(SocialLoginUser user);
        #endregion

        #region Internal
        protected class SocialLoginUser
        {
            public string Id { get; set; }
            public string EmailAddress { get; set; }
            public string Locale { get; set; }
        }
        #endregion
    }
}