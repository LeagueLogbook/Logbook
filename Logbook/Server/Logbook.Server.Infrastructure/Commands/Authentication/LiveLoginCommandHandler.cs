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
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Logbook.Shared.Results;
using Raven.Client;
using Raven.Client.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class LiveLoginCommandHandler : ICommandHandler<LiveLoginCommand, string>
    {
        private readonly IAsyncDocumentSession _documentSession;
        private readonly IJsonWebTokenService _jsonWebTokenService;
        private readonly ILiveService _liveService;

        public LiveLoginCommandHandler(IAsyncDocumentSession documentSession, IJsonWebTokenService jsonWebTokenService, ILiveService liveService)
        {
            this._documentSession = documentSession;
            this._jsonWebTokenService = jsonWebTokenService;
            this._liveService = liveService;
        }

        public async Task<Result<string>> Execute(LiveLoginCommand command, ICommandScope scope)
        {
            string liveToken = await this._liveService.ExchangeCodeForTokenAsync(command.RedirectUrl, command.Code).WithCurrentCulture();

            if (string.IsNullOrWhiteSpace(liveToken))
                return Result.AsError(ServerMessages.InternalServerError);

            var liveUser = await this._liveService.GetMeAsync(liveToken).WithCurrentCulture();

            if (liveUser == null)
                return Result.AsError(ServerMessages.InternalServerError);

            IList<Func<LiveUser, Task<string>>> casesToCheck = new List<Func<LiveUser, Task<string>>>
            {
                this.FindUserIdByLiveUserId,
                this.FindUserIdByEmailAddress,
                this.CreateNewUser
            };

            foreach(var caseToCheck in casesToCheck)
            {
                var userId = await caseToCheck(liveUser).WithCurrentCulture();
                if (userId != null)
                {
                    var jsonWebToken = this._jsonWebTokenService.Generate(userId);
                    return Result.AsSuccess(jsonWebToken);
                }
            }

            return Result.AsError(ServerMessages.InternalServerError);
        }

        private async Task<string> FindUserIdByLiveUserId(LiveUser liveUser)
        {
            var authenticationData = await this._documentSession.Query<AuthenticationData_ByAllFields.Result, AuthenticationData_ByAllFields>()
                .Where(f => f.LiveUserId == liveUser.Id)
                .OfType<AuthenticationData>()
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            return authenticationData?.ForUserId;
        }

        private async Task<string> FindUserIdByEmailAddress(LiveUser liveUser)
        {
            var user = await this._documentSession.Query<User, Users_ByEmailAddress>()
                .Where(f => f.EmailAddress == liveUser.EmailAddress)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            if (user == null)
                return null;

            var authenticationData = await this._documentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
                .WithCurrentCulture();

            authenticationData.Authentications.Add(new LiveAuthenticationKind
            {
                LiveUserId = liveUser.Id
            });

            return user.Id;
        }

        private async Task<string> CreateNewUser(LiveUser liveUser)
        {
            var user = new User
            {
                EmailAddress = liveUser.EmailAddress,
                PreferredLanguage = new CultureInfo(liveUser.Locale).Parent.TwoLetterISOLanguageName
            };

            await this._documentSession.StoreAsync(user).WithCurrentCulture();

            var authenticationData = new AuthenticationData
            {
                ForUserId = user.Id,
                Authentications =
                {
                    new LiveAuthenticationKind
                    {
                        LiveUserId = liveUser.Id
                    }
                }
            };

            await this._documentSession.StoreAsync(authenticationData).WithCurrentCulture();

            return user.Id;
        }
    }
}