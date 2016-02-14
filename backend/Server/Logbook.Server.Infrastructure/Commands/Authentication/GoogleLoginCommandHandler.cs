using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Contracts.Social;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using NHibernate;
using NHibernate.Linq;
using Logbook.Shared;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class GoogleLoginCommandHandler : SocialLoginCommandHandler<GoogleLoginCommand>
    {
        private readonly ISession _session;
        private readonly IGoogleService _googleService;

        public GoogleLoginCommandHandler(ISession session, IJsonWebTokenService jsonWebTokenService, IGoogleService googleService)
            : base(session, jsonWebTokenService)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(jsonWebTokenService, nameof(jsonWebTokenService));
            Guard.NotNull(googleService, nameof(googleService));

            this._session = session;
            this._googleService = googleService;
        }

        protected override async Task<SocialLoginUser> GetMeAsync(GoogleLoginCommand command)
        {
            Guard.NotNull(command, nameof(command));

            var token = await this._googleService.ExchangeCodeForTokenAsync(command.RedirectUrl, command.Code);
            var user = await this._googleService.GetMeAsync(token);

            if (user == null)
                return null;

            return new SocialLoginUser
            {
                EmailAddress = user.EmailAddress,
                Locale = user.Locale,
                Id = user.Id
            };
        }

        protected override User GetUserForSocialUser(SocialLoginUser user)
        {
            Guard.NotNull(user, nameof(user));

            var authentication = this._session.Query<GoogleAuthenticationKind>()
                .Fetch(f => f.User)
                .FirstOrDefault(f => f.GoogleUserId == user.Id);

            return authentication?.User;
        }

        protected override AuthenticationKindBase CreateAuthentication(SocialLoginUser user)
        {
            Guard.NotNull(user, nameof(user));

            return new GoogleAuthenticationKind
            {
                GoogleUserId = user.Id
            };
        }
    }
}