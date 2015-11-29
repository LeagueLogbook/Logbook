using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Infrastructure.Commands;

namespace Logbook.Server.Infrastructure.Windsor
{
    public class CommandInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn(typeof (ICommandHandler<,>))
                .WithServiceFromInterface()
                .LifestyleTransient());

            container.Register(
                Component.For<ICommandExecutor>().ImplementedBy<CommandExecutor>().LifestyleSingleton(),
                Component.For<ICommandScope>().ImplementedBy<CommandScope>().LifestyleScoped());
        }
    }
}