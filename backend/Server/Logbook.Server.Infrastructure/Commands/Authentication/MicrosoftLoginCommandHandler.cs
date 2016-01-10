using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Logbook.Localization.Server;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Contracts.Social;
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Raven.Client;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class MicrosoftLoginCommandHandler : SocialLoginCommandHandler<MicrosoftLoginCommand>
    {
        private readonly IMicrosoftService _microsoftService;

        public MicrosoftLoginCommandHandler(IAsyncDocumentSession documentSession, IJsonWebTokenService jsonWebTokenService, IMicrosoftService microsoftService)
            : base(documentSession, jsonWebTokenService)
        {
            this._microsoftService = microsoftService;
        }

        protected override async Task<SocialLoginUser> GetMeAsync(MicrosoftLoginCommand command)
        {
            var token = await this._microsoftService.ExchangeCodeForTokenAsync(command.RedirectUrl, command.Code);

            var user = await this._microsoftService.GetMeAsync(token).WithCurrentCulture();

            if (user == null)
                return null;

            return new SocialLoginUser
            {
                Id = user.Id,
                Locale = user.Locale,
                EmailAddress = user.EmailAddress
            };
        }

        protected override Expression<Func<AuthenticationData_ByAllFields.Result, bool>> GetExpressionForSocialUserId(SocialLoginUser user)
        {
            return f => f.MicrosoftUserId == user.Id;
        }

        protected override AuthenticationKindBase CreateAuthentication(SocialLoginUser user)
        {
            return new MicrosoftAuthenticationKind
            {
                MicrosoftUserId = user.Id
            };
        }
    }
}