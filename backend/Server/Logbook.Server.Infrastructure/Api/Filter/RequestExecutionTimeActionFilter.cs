using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Logbook.Shared.Common;
using Metrics;
using Timer = Metrics.Timer;

namespace Logbook.Server.Infrastructure.Api.Filter
{
    public class RequestExecutionTimeActionFilter : IActionFilter
    {
        #region Fields
        private readonly Timer _timer = Metric.Timer("Request Execution", Unit.Requests);
        #endregion

        #region Properties
        public bool AllowMultiple => false;
        #endregion

        #region Implementation of IActionFilter
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            bool ignore = 
                actionContext.ActionDescriptor.GetCustomAttributes<DoNotTimeRequestExecutionAttribute>().Any() ||
                actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<DoNotTimeRequestExecutionAttribute>().Any();

            IDisposable disposable = ignore == false 
                ? this._timer.NewContext() 
                : (IDisposable)new NullDisposable();

            using (disposable)
            {
                return await continuation();
            }
        }
        #endregion
    }

    public class DoNotTimeRequestExecutionAttribute : Attribute
    {
        
    }
}