using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
            if (data?.EmailAddress == null || data?.PasswordMD5Hash == null)
                return this.Request.GetMessageWithError(HttpStatusCode.BadRequest, string.Empty);

            var result = await this.CommandExecutor
                .Execute(new RegisterCommand(data.EmailAddress, data.PasswordMD5Hash, data.PreferredLanguage))
                .WithCurrentCulture();

            return this.Request.GetMessageWithResult(HttpStatusCode.Created, HttpStatusCode.InternalServerError, result);
        }
    }
}