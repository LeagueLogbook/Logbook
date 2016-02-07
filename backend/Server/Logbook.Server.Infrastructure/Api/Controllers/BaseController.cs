using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Anotar.NLog;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Microsoft.Owin;

namespace Logbook.Server.Infrastructure.Api.Controllers
{
    public abstract class BaseController : ApiController
    {
        #region Properties
        /// <summary>
        /// Gets the command executor.
        /// </summary>
        public ICommandExecutor CommandExecutor { get; private set; }
        /// <summary>
        /// Gets the owin context.
        /// </summary>
        public IOwinContext OwinContext
        {
            get { return this.Request.GetOwinContext(); }
        }
        /// <summary>
        /// Gets or sets the current user identifier.
        /// </summary>
        public int CurrentUserId
        {
            get
            {
                var id = Thread.CurrentPrincipal?.Identity?.Name;

                return id != null
                    ? int.Parse(id)
                    : 0;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor.</param>
        protected BaseController(ICommandExecutor commandExecutor)
        {
            Guard.AgainstNullArgument(nameof(commandExecutor), commandExecutor);

            this.CommandExecutor = commandExecutor;
        }
        #endregion
    }
}