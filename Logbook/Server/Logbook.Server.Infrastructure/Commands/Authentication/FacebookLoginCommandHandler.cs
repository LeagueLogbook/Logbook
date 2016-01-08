using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Contracts.Social;
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Raven.Client;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class FacebookLoginCommandHandler : SocialLoginCommandHandler<FacebookLoginCommand>
    {
        private readonly IFacebookService _facebookService;

        public FacebookLoginCommandHandler(IAsyncDocumentSession documentSession, IJsonWebTokenService jsonWebTokenService, IFacebookService facebookService)
            : base(documentSession, jsonWebTokenService)
        {
            this._facebookService = facebookService;
        }
        
        protected override async Task<SocialLoginUser> GetMeAsync(FacebookLoginCommand command)
        {
            var token = await this._facebookService.ExchangeCodeForTokenAsync(command.RedirectUrl, command.Code).WithCurrentCulture();

            var user = await this._facebookService.GetMeAsync(token).WithCurrentCulture();

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
            return f => f.FacebookUserId == user.Id;
        }

        protected override AuthenticationKindBase CreateAuthentication(SocialLoginUser user)
        {
            return new FacebookAuthenticationKind
            {
                FacebookUserId = user.Id
            };
        }
    }
}