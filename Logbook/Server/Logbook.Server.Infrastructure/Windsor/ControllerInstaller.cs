using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Server.Infrastructure.Api.Controllers;

namespace Logbook.Server.Infrastructure.Windsor
{
    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<BaseController>()
                .WithServiceSelf()
                .LifestyleScoped());
        }
    }
}