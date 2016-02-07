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

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class GoogleLoginCommandHandler : SocialLoginCommandHandler<GoogleLoginCommand>
    {
        private readonly ISession _session;
        private readonly IGoogleService _googleService;

        public GoogleLoginCommandHandler(ISession session, IJsonWebTokenService jsonWebTokenService, IGoogleService googleService)
            : base(session, jsonWebTokenService)
        {
            this._session = session;
            this._googleService = googleService;
        }

        protected override async Task<SocialLoginUser> GetMeAsync(GoogleLoginCommand command)
        {
            var token = await this._googleService.ExchangeCodeForTokenAsync(command.RedirectUrl, command.Code).WithCurrentCulture();
            var user = await this._googleService.GetMeAsync(token).WithCurrentCulture();

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
            var authentication = this._session.Query<GoogleAuthenticationKind>()
                .Fetch(f => f.User)
                .FirstOrDefault(f => f.GoogleUserId == user.Id);

            return authentication?.User;
        }

        protected override AuthenticationKindBase CreateAuthentication(SocialLoginUser user)
        {
            return new GoogleAuthenticationKind
            {
                GoogleUserId = user.Id
            };
        }
    }
}