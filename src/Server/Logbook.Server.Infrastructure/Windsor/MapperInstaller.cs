using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Server.Contracts.Mapping;

namespace Logbook.Server.Infrastructure.Windsor
{
    public class MapperInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn(typeof (IMapper<,>))
                .WithServiceFromInterface()
                .LifestyleTransient());
        }
    }
}