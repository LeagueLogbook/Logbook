using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Server.Contracts;

namespace Logbook.Worker.AnalyzeSummonerMatchHistory.Windsor
{
    public class WorkerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<AnalyzeSummonerMatchHistoryWorker>().Forward<IWorker>().LifestyleSingleton());
        }
    }
}