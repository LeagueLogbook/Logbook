using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Users;
using Logbook.Server.Infrastructure.Api.Filter;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Extensions;

namespace Logbook.Server.Infrastructure.Api.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
        }

        [HttpGet]
        [Route("Users/Me")]
        [LogbookAuthentication]
        public async Task<HttpResponseMessage> GetMeAsync()
        {
            var user = await this.CommandExecutor
                .Execute(new GetUserCommand(this.CurrentUserId))
                .WithCurrentCulture();

            return this.Request.GetMessageWithObject(HttpStatusCode.Found, user);
        }
    }
}