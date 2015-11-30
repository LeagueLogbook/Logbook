using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Admin;
using Logbook.Server.Infrastructure.Api.Filter;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Extensions;

namespace Logbook.Server.Infrastructure.Api.Controllers
{
    [RoutePrefix("Admin")]
    public class AdminController : BaseController
    {
        public AdminController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
        }

        [HttpPost]
        [Route("Indexes")]
        [OnlyLocal]
        public async Task<HttpResponseMessage> CreateIndexes()
        {
            var result = await this.CommandExecutor
                .Execute(new CreateIndexesCommand())
                .WithCurrentCulture();

            return this.Request.GetMessageWithResult(HttpStatusCode.Created, HttpStatusCode.InternalServerError, result, ignoreData: true);
        }

        [HttpPatch]
        [Route("Indexes")]
        [OnlyLocal]
        public async Task<HttpResponseMessage> ResetIndexes()
        {
            var result = await this.CommandExecutor
                .Execute(new ResetIndexesCommand())
                .WithCurrentCulture();

            return this.Request.GetMessageWithResult(HttpStatusCode.OK, HttpStatusCode.InternalServerError, result, ignoreData: true);
        }
    }
}