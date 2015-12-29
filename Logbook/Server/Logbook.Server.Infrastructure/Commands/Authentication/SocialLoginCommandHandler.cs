using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Logbook.Shared.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public abstract class SocialLoginCommandHandler<TCommand> : ICommandHandler<TCommand, JsonWebToken>
        where TCommand : ICommand<JsonWebToken>
    {
        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        private readonly IJsonWebTokenService _jsonWebTokenService;
        #endregion

        #region Constructors
        protected SocialLoginCommandHandler(IAsyncDocumentSession documentSession, IJsonWebTokenService jsonWebTokenService)
        {
            Guard.AgainstNullArgument(nameof(documentSession), documentSession);
            Guard.AgainstNullArgument(nameof(jsonWebTokenService), jsonWebTokenService);

            this._documentSession = documentSession;
            this._jsonWebTokenService = jsonWebTokenService;
        }
        #endregion

        #region Methods
        public async Task<JsonWebToken> Execute(TCommand command, ICommandScope scope)
        {
            string token = await this.ExchangeCodeForTokenAsync(command).WithCurrentCulture();

            if (string.IsNullOrWhiteSpace(token))
                throw new InternalServerErrorException();

            var socialLoginUser = await this.GetMeAsync(token).WithCurrentCulture();

            if (socialLoginUser == null)
                throw new InternalServerErrorException();

            IList<Func<SocialLoginUser, Task<string>>> casesToCheck = new List<Func<SocialLoginUser, Task<string>>>
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
                    return this._jsonWebTokenService.Generate(userId);
                }
            }

            throw new InternalServerErrorException();
        }
        #endregion

        #region Private Methods
        private async Task<string> FindUserIdBySocialLoginUserId(SocialLoginUser socialLoginUser)
        {
            var authenticationData = await this._documentSession.Query<AuthenticationData_ByAllFields.Result, AuthenticationData_ByAllFields>()
                .Where(this.GetExpressionForSocialUserId(socialLoginUser))
                .OfType<AuthenticationData>()
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            return authenticationData?.ForUserId;
        }

        private async Task<string> FindUserIdByEmailAddress(SocialLoginUser socialLoginUser)
        {
            var user = await this._documentSession.Query<User, Users_ByEmailAddress>()
                .Where(f => f.EmailAddress == socialLoginUser.EmailAddress)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            if (user == null)
                return null;

            var authenticationData = await this._documentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
                .WithCurrentCulture();

            authenticationData.Authentications.Add(this.CreateAuthentication(socialLoginUser));

            return user.Id;
        }

        private async Task<string> CreateNewUser(SocialLoginUser socialLoginUser)
        {
            var user = new User
            {
                EmailAddress = socialLoginUser.EmailAddress,
                PreferredLanguage = new CultureInfo(socialLoginUser.Locale).Parent.TwoLetterISOLanguageName
            };

            await this._documentSession.StoreAsync(user).WithCurrentCulture();

            var authenticationData = new AuthenticationData
            {
                ForUserId = user.Id,
                Authentications =
                {
                    this.CreateAuthentication(socialLoginUser),
                }
            };

            await this._documentSession.StoreAsync(authenticationData).WithCurrentCulture();

            return user.Id;
        }
        
        protected abstract Task<string> ExchangeCodeForTokenAsync(TCommand command);

        protected abstract Task<SocialLoginUser> GetMeAsync(string token);

        protected abstract Expression<Func<AuthenticationData_ByAllFields.Result, bool>> GetExpressionForSocialUserId(SocialLoginUser user);

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