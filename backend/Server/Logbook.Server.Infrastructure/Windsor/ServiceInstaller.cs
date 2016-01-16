using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Server.Contracts;
using RiotSharp;

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
                .LifestyleTransient());

            container.Register(
                Component.For<IRiotApi>().UsingFactoryMethod((kernel, context) => RiotApi.GetInstance(Config.RiotApiKey, Config.RiotApiRateLimitPer10Seconds, Config.RiotAPiRateLimitPer10Minutes)).LifestyleTransient(),
                Component.For<IStaticRiotApi>().UsingFactoryMethod((kernel, context) => StaticRiotApi.GetInstance(Config.RiotApiKey)).LifestyleTransient());
        }
    }
}