using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Server.Contracts;

namespace Logbook.Worker.UpdateSummoners.Windsor
{
    public class WorkerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<UpdateSummonersWorker>().Forward<IWorker>().LifestyleSingleton());
        }
    }
}