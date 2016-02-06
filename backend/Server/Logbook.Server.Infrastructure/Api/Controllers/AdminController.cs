using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Logbook.Server.Contracts.Commands;
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
        
    }
}