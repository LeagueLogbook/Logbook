using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Contracts.Social;
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Authentication;
using Raven.Client;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class TwitterLoginCommandHandler : SocialLoginCommandHandler<TwitterLoginCommand>
    {
        private readonly ITwitterService _twitterService;

        public TwitterLoginCommandHandler(IAsyncDocumentSession documentSession, IJsonWebTokenService jsonWebTokenService, ITwitterService twitterService)
            : base(documentSession, jsonWebTokenService)
        {
            this._twitterService = twitterService;
        }

        protected override async Task<SocialLoginUser> GetMeAsync(TwitterLoginCommand command)
        {
            var token = await this._twitterService.ExchangeForToken(command.Payload, command.OAuthVerifier);
            var user = await this._twitterService.GetMeAsync(token);

            if (user == null)
                return null;

            return new SocialLoginUser
            {
                Id = user.Id,
                Locale = user.Locale,
                EmailAddress = user.Email
            };
        }

        protected override Expression<Func<AuthenticationData_ByAllFields.Result, bool>> GetExpressionForSocialUserId(SocialLoginUser user)
        {
            return f => f.TwitterUserId == user.Id;
        }

        protected override AuthenticationKindBase CreateAuthentication(SocialLoginUser user)
        {
            return new TwitterAuthenticationKind
            {
                TwitterUserId = user.Id
            };
        }
    }
}