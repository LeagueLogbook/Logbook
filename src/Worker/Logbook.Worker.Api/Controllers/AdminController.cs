using System.Web.Http;
using Logbook.Server.Contracts.Commands;
using Logbook.Shared;

namespace Logbook.Worker.Api.Controllers
{
    [RoutePrefix("Admin")]
    public class AdminController : BaseController
    {
        public AdminController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
            Guard.NotNull(commandExecutor, nameof(commandExecutor));
        }
    }
}