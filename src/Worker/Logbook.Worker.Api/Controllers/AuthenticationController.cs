﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Social;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared;
using Logbook.Shared.Extensions;
using Logbook.Worker.Api.Extensions;
using Logbook.Worker.Api.Models;

namespace Logbook.Worker.Api.Controllers
{
    [RoutePrefix("Authentication")]
    public class AuthenticationController : BaseController
    {
        private readonly IMicrosoftService _microsoftService;
        private readonly IFacebookService _facebookService;
        private readonly IGoogleService _googleService;
        private readonly ITwitterService _twitterService;

        public AuthenticationController(ICommandExecutor commandExecutor, IMicrosoftService microsoftService, IFacebookService facebookService, IGoogleService googleService, ITwitterService twitterService)
            : base(commandExecutor)
        {
            Guard.NotNull(commandExecutor, nameof(commandExecutor));
            Guard.NotNull(microsoftService, nameof(microsoftService));
            Guard.NotNull(facebookService, nameof(facebookService));
            Guard.NotNull(googleService, nameof(googleService));
            Guard.NotNull(twitterService, nameof(twitterService));

            this._microsoftService = microsoftService;
            this._facebookService = facebookService;
            this._googleService = googleService;
            this._twitterService = twitterService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<HttpResponseMessage> RegisterAsync(RegisterData data)
        {
            if (data?.EmailAddress == null || data?.PasswordSHA256Hash == null)
                throw new DataMissingException();

            await this.CommandExecutor
                .Execute(new RegisterCommand(data.EmailAddress, Convert.FromBase64String(data.PasswordSHA256Hash), data.PreferredLanguage, this.OwinContext));

            return this.Request.GetMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("Register/Finish")]
        public async Task<HttpResponseMessage> FinishRegistrationAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new DataMissingException();

            await this.CommandExecutor
                .Execute(new FinishRegistrationCommand(token));

            return this.Request.GetMessage(HttpStatusCode.Created);
        }

        [HttpPost]
        [Route("PasswordReset")]
        public async Task<HttpResponseMessage> RequestPasswordResetAsync(PasswordResetData data)
        {
            if (data?.EmailAddress == null)
                throw new DataMissingException();

            await this.CommandExecutor
                .Execute(new ResetPasswordCommand(data.EmailAddress, this.OwinContext));

            return this.Request.GetMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("PasswordReset/Finish")]
        public async Task<HttpResponseMessage> FinishPasswordResetAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new DataMissingException();

            await this.CommandExecutor
                .Execute(new FinishPasswordResetCommand(token));

            return this.Request.GetMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("Login/Logbook")]
        public async Task<HttpResponseMessage> LoginAsync(LoginData data)
        {
            if (data?.EmailAddress == null || data?.PasswordSHA256Hash == null)
                throw new DataMissingException();

            var token = await this.CommandExecutor
                .Execute(new LoginCommand(data.EmailAddress, Convert.FromBase64String(data.PasswordSHA256Hash)));

            return this.Request.GetMessageWithObject(HttpStatusCode.OK, token);
        }

        [HttpGet]
        [Route("Login/Microsoft/Url")]
        public async Task<HttpResponseMessage> GetLoginLiveUrlAsync(string redirectUrl)
        {
            if (string.IsNullOrWhiteSpace(redirectUrl))
                throw new DataMissingException();

            var url = await this._microsoftService.GetLoginUrlAsync(redirectUrl);
            return this.Request.GetMessageWithObject(HttpStatusCode.OK, new {Url = url});
        }

        [HttpPost]
        [Route("Login/Microsoft")]
        public async Task<HttpResponseMessage> LoginLiveAsync(MicrosoftLoginData data)
        {
            if (data?.Code == null || data?.RedirectUrl == null)
                throw new DataMissingException();

            var token = await this.CommandExecutor
                .Execute(new MicrosoftLoginCommand(data.Code, data.RedirectUrl));

            return this.Request.GetMessageWithObject(HttpStatusCode.OK, token);
        }

        [HttpGet]
        [Route("Login/Facebook/Url")]
        public async Task<HttpResponseMessage> GetLoginFacebookUrlAsync(string redirectUrl)
        {
            if (string.IsNullOrWhiteSpace(redirectUrl))
                throw new DataMissingException();

            var url = await this._facebookService.GetLoginUrlAsync(redirectUrl);
            return this.Request.GetMessageWithObject(HttpStatusCode.OK, new {Url = url});
        }

        [HttpPost]
        [Route("Login/Facebook")]
        public async Task<HttpResponseMessage> LoginFacebookAsync(FacebookLoginData data)
        {
            if (data?.Code == null || data?.RedirectUrl == null)
                throw new DataMissingException();

            var token = await this.CommandExecutor
                .Execute(new FacebookLoginCommand(data.Code, data.RedirectUrl));

            return this.Request.GetMessageWithObject(HttpStatusCode.OK, token);
        }

        [HttpGet]
        [Route("Login/Google/Url")]
        public async Task<HttpResponseMessage> GetLoginGoogleUrlAsync(string redirectUrl)
        {
            if (string.IsNullOrWhiteSpace(redirectUrl))
                throw new DataMissingException();

            var url = await this._googleService.GetLoginUrlAsync(redirectUrl);
            return this.Request.GetMessageWithObject(HttpStatusCode.OK, new {Url = url});
        }

        [HttpPost]
        [Route("Login/Google")]
        public async Task<HttpResponseMessage> LoginGoogleAsync(GoogleLoginData data)
        {
            if (data?.Code == null || data?.RedirectUrl == null)
                throw new DataMissingException();

            var token = await this.CommandExecutor
                .Execute(new GoogleLoginCommand(data.Code, data.RedirectUrl));

            return this.Request.GetMessageWithObject(HttpStatusCode.OK, token);
        }

        [HttpGet]
        [Route("Login/Twitter/Url")]
        public async Task<HttpResponseMessage> GetLoginTwitterUrlAsync(string redirectUrl)
        {
            if (string.IsNullOrWhiteSpace(redirectUrl))
                throw new DataMissingException();

            var url = await this._twitterService.GetLoginUrlAsync(redirectUrl);
            return this.Request.GetMessageWithObject(HttpStatusCode.OK, url);
        }

        [HttpPost]
        [Route("Login/Twitter")]
        public async Task<HttpResponseMessage> LoginTwitterAsync(TwitterLoginData data)
        {
            if (data?.Payload == null || data?.OAuthVerifier == null)
                throw new DataMissingException();

            var token = await this.CommandExecutor
                .Execute(new TwitterLoginCommand(data.OAuthVerifier, data.Payload));

            return this.Request.GetMessageWithObject(HttpStatusCode.OK, token);
        }
    }
}