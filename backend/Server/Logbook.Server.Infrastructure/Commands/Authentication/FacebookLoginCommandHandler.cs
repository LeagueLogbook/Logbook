using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Contracts.Social;
using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class FacebookLoginCommandHandler : SocialLoginCommandHandler<FacebookLoginCommand>
    {
        private readonly ISession _session;
        private readonly IFacebookService _facebookService;

        public FacebookLoginCommandHandler(ISession session, IJsonWebTokenService jsonWebTokenService, IFacebookService facebookService)
            : base(session, jsonWebTokenService)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(jsonWebTokenService, nameof(jsonWebTokenService));
            Guard.NotNull(facebookService, nameof(facebookService));

            this._session = session;
            this._facebookService = facebookService;
        }

        protected override async Task<SocialLoginUser> GetMeAsync(FacebookLoginCommand command)
        {
            Guard.NotNull(command, nameof(command));

            var token = await this._facebookService.ExchangeCodeForTokenAsync(command.RedirectUrl, command.Code);

            var user = await this._facebookService.GetMeAsync(token);

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

            var authentication = this._session.Query<FacebookAuthenticationKind>()
                .Fetch(f => f.User)
                .FirstOrDefault(f => f.FacebookUserId == user.Id);

            return authentication?.User;
        }

        protected override AuthenticationKindBase CreateAuthentication(SocialLoginUser user)
        {
            Guard.NotNull(user, nameof(user));

            return new FacebookAuthenticationKind
            {
                FacebookUserId = user.Id
            };
        }
    }
}