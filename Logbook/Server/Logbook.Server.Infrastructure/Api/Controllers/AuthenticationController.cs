using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Logbook.Localization.Server;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Extensions;
using Logbook.Shared.Models;

namespace Logbook.Server.Infrastructure.Api.Controllers
{
    [RoutePrefix("Authentication")]
    public class AuthenticationController : BaseController
    {
        public AuthenticationController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
        }

        [HttpPost]
        [Route("Register")]
        public async Task<HttpResponseMessage> RegisterAsync(RegisterData data)
        {
            if (data?.EmailAddress == null || data?.PasswordSHA256Hash == null)
                return this.Request.GetMessageWithError(HttpStatusCode.BadRequest, ServerMessages.DataMissing);

            var result = await this.CommandExecutor
                .Execute(new RegisterCommand(data.EmailAddress, data.PasswordSHA256Hash, data.PreferredLanguage))
                .WithCurrentCulture();

            return this.Request.GetMessageWithResult(HttpStatusCode.Created, HttpStatusCode.InternalServerError, result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<HttpResponseMessage> LoginAsync(LoginData data)
        {
            if (data?.EmailAddress == null || data?.PasswordSHA256Hash == null)
                return this.Request.GetMessageWithError(HttpStatusCode.BadRequest, ServerMessages.DataMissing);

            var result = await this.CommandExecutor
                .Execute(new LoginCommand(data.EmailAddress, data.PasswordSHA256Hash))
                .WithCurrentCulture();

            return this.Request.GetMessageWithResult(HttpStatusCode.OK, HttpStatusCode.InternalServerError, result);
        }
    }
}