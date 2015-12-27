using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Localization.Server;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Contracts.Social;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Logbook.Shared.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class MicrosoftLoginCommandHandler : ICommandHandler<MicrosoftLoginCommand, AuthenticationToken>
    {
        private readonly IAsyncDocumentSession _documentSession;
        private readonly IJsonWebTokenService _jsonWebTokenService;
        private readonly IMicrosoftService _microsoftService;

        public MicrosoftLoginCommandHandler(IAsyncDocumentSession documentSession, IJsonWebTokenService jsonWebTokenService, IMicrosoftService microsoftService)
        {
            this._documentSession = documentSession;
            this._jsonWebTokenService = jsonWebTokenService;
            this._microsoftService = microsoftService;
        }

        public async Task<AuthenticationToken> Execute(MicrosoftLoginCommand command, ICommandScope scope)
        {
            string liveToken = await this._microsoftService.ExchangeCodeForTokenAsync(command.RedirectUrl, command.Code).WithCurrentCulture();

            if (string.IsNullOrWhiteSpace(liveToken))
                throw new InternalServerErrorException();

            var liveUser = await this._microsoftService.GetMeAsync(liveToken).WithCurrentCulture();

            if (liveUser == null)
                throw new InternalServerErrorException();

            IList<Func<MicrosoftUser, Task<string>>> casesToCheck = new List<Func<MicrosoftUser, Task<string>>>
            {
                this.FindUserIdByMicrosoftUserId,
                this.FindUserIdByEmailAddress,
                this.CreateNewUser
            };

            foreach(var caseToCheck in casesToCheck)
            {
                var userId = await caseToCheck(liveUser).WithCurrentCulture();
                if (userId != null)
                {
                    return this._jsonWebTokenService.Generate(userId);
                }
            }

            throw new InternalServerErrorException();
        }

        private async Task<string> FindUserIdByMicrosoftUserId(MicrosoftUser microsoftUser)
        {
            var authenticationData = await this._documentSession.Query<AuthenticationData_ByAllFields.Result, AuthenticationData_ByAllFields>()
                .Where(f => f.MicrosoftUserId == microsoftUser.Id)
                .OfType<AuthenticationData>()
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            return authenticationData?.ForUserId;
        }

        private async Task<string> FindUserIdByEmailAddress(MicrosoftUser microsoftUser)
        {
            var user = await this._documentSession.Query<User, Users_ByEmailAddress>()
                .Where(f => f.EmailAddress == microsoftUser.EmailAddress)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            if (user == null)
                return null;

            var authenticationData = await this._documentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
                .WithCurrentCulture();

            authenticationData.Authentications.Add(new MicrosoftAuthenticationKind
            {
                MicrosoftUserId = microsoftUser.Id
            });

            return user.Id;
        }

        private async Task<string> CreateNewUser(MicrosoftUser microsoftUser)
        {
            var user = new User
            {
                EmailAddress = microsoftUser.EmailAddress,
                PreferredLanguage = new CultureInfo(microsoftUser.Locale).Parent.TwoLetterISOLanguageName
            };

            await this._documentSession.StoreAsync(user).WithCurrentCulture();

            var authenticationData = new AuthenticationData
            {
                ForUserId = user.Id,
                Authentications =
                {
                    new MicrosoftAuthenticationKind
                    {
                        MicrosoftUserId = microsoftUser.Id
                    }
                }
            };

            await this._documentSession.StoreAsync(authenticationData).WithCurrentCulture();

            return user.Id;
        }
    }
}