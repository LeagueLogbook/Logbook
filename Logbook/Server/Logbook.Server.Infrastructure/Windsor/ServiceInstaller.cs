using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Server.Contracts;

namespace Logbook.Server.Infrastructure.Windsor
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<IService>()
                .WithServiceFromInterface()
                .LifestyleSingleton());
        }
    }
}