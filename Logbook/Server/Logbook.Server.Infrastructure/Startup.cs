using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Logbook.Server.Infrastructure.Api.Configuration;
using Logbook.Server.Infrastructure.Api.MessageHandlers;
using Logbook.Server.Infrastructure.Windsor;
using Metrics;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Compressors;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Logbook.Server.Infrastructure
{
    public class Startup
    {
        #region Methods
        /// <summary>
        /// Configures the OWIN webservice host.
        /// </summary>
        /// <param name="app">The application builder.</param>
        public void Configuration(IAppBuilder app)
        {
            this.UseCors(app);
            this.UseWebApi(app);

            this.ConfigureDefaultMetrics();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Configures the default metrics.
        /// </summary>
        private void ConfigureDefaultMetrics()
        {
            if (Config.EnableDefaultMetrics)
            {
                Metric.Config.WithAllCounters();
            }
        }
        /// <summary>
        /// Makes the app use cors.
        /// </summary>
        /// <param name="app">The application.</param>
        private void UseCors(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
        }
        /// <summary>
        /// Makes the app use webapi.
        /// </summary>
        /// <param name="app">The application.</param>
        private void UseWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            this.ConfigureWindsor(config);
            this.ConfigureFilters(config);
            this.ConfigureMessageHandlers(config);
            this.ConfigureServices(config);
            this.ConfigureRoutes(config);
            this.ConfigureJson(config);

            app.UseWebApi(config);
        }
        /// <summary>
        /// Configures the castle windsor container.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureWindsor(HttpConfiguration config)
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());

            config.DependencyResolver = new WindsorResolver(container);
        }
        /// <summary>
        /// Configures the default filters.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureFilters(HttpConfiguration config)
        {
        }
        /// <summary>
        /// Configures the message handlers.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureMessageHandlers(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new RequestExecutionTimeMessageHandler());
            config.MessageHandlers.Add(new ConcurrentRequestCountMessageHandler());

            if (Config.CompressResponses.GetValue())
            {
                config.MessageHandlers.Add(new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
            }

            config.MessageHandlers.Add(new LocalizationMessageHandler());

            if (Config.EnableDebugRequestResponseLogging.GetValue())
            {
                config.MessageHandlers.Add(new LoggingMessageHandler());
            }
        }
        /// <summary>
        /// Configures the WebAPI services.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureServices(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IAssembliesResolver), new LogbookAssembliesResolver());
            config.Services.Replace(typeof(IExceptionHandler), new LogbookExceptionHandler());
            config.Services.Replace(typeof(IExceptionLogger), new LogbookExceptionLogger());
        }
        /// <summary>
        /// Configures the routes.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
        /// <summary>
        /// Configures the config to allow only json requests.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureJson(HttpConfiguration config)
        {
            if (Config.FormatResponses.GetValue())
            {
                config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            }

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(config.Formatters.JsonFormatter));
        }
        #endregion
    }
}