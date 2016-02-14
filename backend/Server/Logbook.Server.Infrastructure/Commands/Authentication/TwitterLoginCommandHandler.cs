using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Contracts.Social;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class TwitterLoginCommandHandler : SocialLoginCommandHandler<TwitterLoginCommand>
    {
        private readonly ISession _session;
        private readonly ITwitterService _twitterService;


        public TwitterLoginCommandHandler(ISession session, IJsonWebTokenService jsonWebTokenService, ITwitterService twitterService)
            : base(session, jsonWebTokenService)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(jsonWebTokenService, nameof(jsonWebTokenService));
            Guard.NotNull(twitterService, nameof(twitterService));

            this._session = session;
            this._twitterService = twitterService;
        }

        protected override async Task<SocialLoginUser> GetMeAsync(TwitterLoginCommand command)
        {
            Guard.NotNull(command, nameof(command));

            var token = await this._twitterService.ExchangeForToken(command.Payload, command.OAuthVerifier);

            if (token == null)
                throw new InternalServerErrorException();

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

        protected override User GetUserForSocialUser(SocialLoginUser user)
        {
            Guard.NotNull(user, nameof(user));

            var authentication = this._session.Query<TwitterAuthenticationKind>()
                .Fetch(f => f.User)
                .FirstOrDefault(f => f.TwitterUserId == user.Id);

            return authentication?.User;
        }

        protected override AuthenticationKindBase CreateAuthentication(SocialLoginUser user)
        {
            Guard.NotNull(user, nameof(user));

            return new TwitterAuthenticationKind
            {
                TwitterUserId = user.Id
            };
        }
    }
}