using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Logbook.Localization.Server;
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
    public class MicrosoftLoginCommandHandler : SocialLoginCommandHandler<MicrosoftLoginCommand>
    {
        private readonly ISession _session;
        private readonly IMicrosoftService _microsoftService;

        public MicrosoftLoginCommandHandler(ISession session, IJsonWebTokenService jsonWebTokenService, IMicrosoftService microsoftService)
            : base(session, jsonWebTokenService)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(jsonWebTokenService, nameof(jsonWebTokenService));
            Guard.NotNull(microsoftService, nameof(microsoftService));

            this._session = session;
            this._microsoftService = microsoftService;
        }

        protected override async Task<SocialLoginUser> GetMeAsync(MicrosoftLoginCommand command)
        {
            Guard.NotNull(command, nameof(command));

            var token = await this._microsoftService.ExchangeCodeForTokenAsync(command.RedirectUrl, command.Code);

            var user = await this._microsoftService.GetMeAsync(token);

            if (user == null)
                return null;

            return new SocialLoginUser
            {
                Id = user.Id,
                Locale = user.Locale,
                EmailAddress = user.EmailAddress
            };
        }

        protected override User GetUserForSocialUser(SocialLoginUser user)
        {
            Guard.NotNull(user, nameof(user));

            var authentication = this._session.Query<MicrosoftAuthenticationKind>()
                .Fetch(f => f.User)
                .FirstOrDefault(f => f.MicrosoftUserId == user.Id);

            return authentication?.User;
        }

        protected override AuthenticationKindBase CreateAuthentication(SocialLoginUser user)
        {
            Guard.NotNull(user, nameof(user));

            return new MicrosoftAuthenticationKind
            {
                MicrosoftUserId = user.Id
            };
        }
    }
}