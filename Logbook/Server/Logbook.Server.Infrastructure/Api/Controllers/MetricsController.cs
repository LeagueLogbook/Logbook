using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Infrastructure.Api.Filter;
using Logbook.Server.Infrastructure.Extensions;
using Metrics;
using Metrics.Json;
using Metrics.MetricData;
using Newtonsoft.Json;

namespace Logbook.Server.Infrastructure.Api.Controllers
{
    public class MetricsController : BaseController
    {
        #region Fields
        private static MetricsDataProvider _dataProvider;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the <see cref="MetricsController"/> class.
        /// </summary>
        static MetricsController()
        {
            Metric.Config.WithConfigExtension((context, _) => _dataProvider = context.DataProvider);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MetricsController"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor.</param>
        public MetricsController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
            
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the application metrics.
        /// </summary>
        [HttpGet]
        [Route("Metrics")]
        public HttpResponseMessage GetMetricsAsync()
        {
            string json = JsonBuilderV2.BuildJson(_dataProvider.CurrentMetricsData);
            var obj = JsonConvert.DeserializeObject(json);

            return this.Request.GetMessageWithObject(HttpStatusCode.Found, obj);
        }
        #endregion
    }
}