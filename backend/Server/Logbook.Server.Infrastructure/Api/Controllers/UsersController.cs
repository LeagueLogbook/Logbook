using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Users;
using Logbook.Server.Infrastructure.Api.Filter;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Logbook.Shared.Models.Authentication;

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
            var result = await this.CommandExecutor.Batch(async scope =>
            {
                var user = await scope.Execute(new GetUserCommand(this.CurrentUserId));
                return await scope.Map<User, UserModel>(user);
            });

            return this.Request.GetMessageWithObject(HttpStatusCode.Found, result);
        }
    }
}