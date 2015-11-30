using System.Net.Http;
using System.Web.Http;
using Anotar.NLog;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Infrastructure.Api.Data;
using Microsoft.Owin;

namespace Logbook.Server.Infrastructure.Api.Controllers
{
    public abstract class BaseController : ApiController
    {
        #region Constants
        public const int DefaultSkip = 0;
        public const int DefaultTake = 50;
        #endregion

        #region Fields
        private PagingInfo _paging;
        #endregion

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
        /// Returns the paging informations.
        /// </summary>
        public PagingInfo Paging
        {
            get
            {
                if (this._paging != null)
                    return this._paging;

                LogTo.Debug("Trying to get the paging information.");

                string skipString = this.OwinContext.Request.Query.Get("skip");

                LogTo.Debug("Query value 'skip' is {0}.", skipString);

                int skip;
                if (int.TryParse(skipString, out skip) == false)
                {
                    LogTo.Debug("Could not parse 'skip' query value. Using default value of {0}.", DefaultSkip);
                    skip = DefaultSkip;
                }

                string takeString = this.OwinContext.Request.Query.Get("take");

                LogTo.Debug("Query value 'take' is {0}.", takeString);

                int take;
                if (int.TryParse(takeString, out take) == false)
                {
                    LogTo.Debug("Could not parse 'take' query value. Using default value of {0}.", DefaultTake);
                    take = DefaultTake;
                }

                LogTo.Debug("Parsed paging information. Skip: {0} Take: {1}", skip, take);

                return this._paging = new PagingInfo(skip, take);
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