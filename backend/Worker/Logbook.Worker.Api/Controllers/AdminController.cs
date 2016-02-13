using System.Web.Http;
using Logbook.Server.Contracts.Commands;

namespace Logbook.Worker.Api.Controllers
{
    [RoutePrefix("Admin")]
    public class AdminController : BaseController
    {
        public AdminController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
        }
    }
}