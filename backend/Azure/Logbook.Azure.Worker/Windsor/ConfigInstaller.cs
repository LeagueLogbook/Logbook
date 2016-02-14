using AppConfigFacility;
using AppConfigFacility.Azure;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Server.Infrastructure;
using Logbook.Server.Infrastructure.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Logbook.Azure.Worker.Windsor
{
    public class ConfigInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<AppConfigFacility.AppConfigFacility>(f => f.FromAzure());
            container.Register(
                Component.For<IHttpConfig>().FromAppConfig(f => f.WithPrefix("Logbook/Http/").Computed(d => d.Address, d => this.GetAddress())),
                Component.For<IEmailConfig>().FromAppConfig(f => f.WithPrefix("Logbook/Email/")),
                Component.For<IAppConfig>().FromAppConfig(f => f.WithPrefix("Logbook/App/")),
                Component.For<IEmailTemplateConfig>().FromAppConfig(f => f.WithPrefix("Logbook/EmailTemplate/")),
                Component.For<ISecurityConfig>().FromAppConfig(f => f.WithPrefix("Logbook/Security/")),
                Component.For<IRiotConfig>().FromAppConfig(f => f.WithPrefix("Logbook/Riot/")),
                Component.For<IDatabaseConfig>().FromAppConfig(f => f.WithPrefix("Logbook/Database/")));

            Config.Http = container.Resolve<IHttpConfig>();
            Config.Email = container.Resolve<IEmailConfig>();
            Config.App = container.Resolve<IAppConfig>();
            Config.EmailTemplate = container.Resolve<IEmailTemplateConfig>();
            Config.Security = container.Resolve<ISecurityConfig>();
            Config.Riot = container.Resolve<IRiotConfig>();
            Config.Database = container.Resolve<IDatabaseConfig>();
        }

        private string GetAddress()
        {
            var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["HTTP"];
            return $"{endpoint.Protocol}://{endpoint.IPEndpoint}";
        }
    }
}