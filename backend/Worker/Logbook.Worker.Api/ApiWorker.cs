using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Extensions.Compression.Core.Compressors;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Logbook.Server.Contracts;
using Logbook.Server.Infrastructure;
using Logbook.Server.Infrastructure.Windsor;
using Logbook.Shared;
using Logbook.Worker.Api.Configuration;
using Logbook.Worker.Api.MessageHandlers;
using Logbook.Worker.Api.Windsor;
using Microsoft.AspNet.WebApi.Extensions.Compression.Server;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Logbook.Worker.Api
{
    public class ApiWorker : IWorker
    {
        #region Fields
        private readonly IWindsorContainer _windsorContainer;

        private IDisposable _webApp;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiWorker"/> class.
        /// </summary>
        /// <param name="windsorContainer">The windsor container.</param>
        public ApiWorker(IWindsorContainer windsorContainer)
        {
            Guard.NotNull(windsorContainer, nameof(windsorContainer));

            this._windsorContainer = windsorContainer;
        }
        #endregion

        #region Implementation of IWorker
        /// <summary>
        /// Startup code for the worker.
        /// </summary>
        /// <returns></returns>
        public Task StartAsync()
        {
            var startOptions = new StartOptions();
            foreach (var url in Config.Addresses.GetValue())
            {
                startOptions.Urls.Add(url.ToString());
            }

            this._webApp = WebApp.Start(startOptions, this.ConfigureHttpApi);

            return Task.CompletedTask;
        }
        /// <summary>
        /// The action the worker is actually doing.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            Guard.NotNull(cancellationToken, nameof(cancellationToken));

            var delay = TimeSpan.FromSeconds(2);
            while (cancellationToken.IsCancellationRequested == false)
            {
                await Task.Delay(delay, cancellationToken);
            }

            this._webApp?.Dispose();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Configures the http api.
        /// </summary>
        /// <param name="app">The application.</param>
        private void ConfigureHttpApi(IAppBuilder app)
        {
            Guard.NotNull(app, nameof(app));

            this.UseCors(app);
            this.UseWebApi(app);
        }
        /// <summary>
        /// Makes the app use cors.
        /// </summary>
        /// <param name="app">The application.</param>
        private void UseCors(IAppBuilder app)
        {
            Guard.NotNull(app, nameof(app));

            app.UseCors(CorsOptions.AllowAll);
        }
        /// <summary>
        /// Makes the app use webapi.
        /// </summary>
        /// <param name="app">The application.</param>
        private void UseWebApi(IAppBuilder app)
        {
            Guard.NotNull(app, nameof(app));

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
            Guard.NotNull(config, nameof(config));

            config.DependencyResolver = new WindsorResolver(this._windsorContainer);
        }
        /// <summary>
        /// Configures the default filters.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureFilters(HttpConfiguration config)
        {
            Guard.NotNull(config, nameof(config));
        }
        /// <summary>
        /// Configures the message handlers.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureMessageHandlers(HttpConfiguration config)
        {
            Guard.NotNull(config, nameof(config));

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
            Guard.NotNull(config, nameof(config));

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
            Guard.NotNull(config, nameof(config));

            config.MapHttpAttributeRoutes();
        }
        /// <summary>
        /// Configures the config to allow only json requests.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureJson(HttpConfiguration config)
        {
            Guard.NotNull(config, nameof(config));

            if (Config.FormatResponses.GetValue())
            {
                config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            }

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(config.Formatters.JsonFormatter));
        }
        #endregion
    }
}
