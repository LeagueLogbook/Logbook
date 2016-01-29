using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Entities.Summoners;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.FileSystem;
using Raven.Database.Config;
using Raven.Server;

namespace Logbook.Server.Infrastructure.Windsor
{
    public class RavenInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            DocumentStore documentStore = this.CreateRavenDocumentStore();
            this.CustomizeRavenDocumentStore(documentStore);

            container.Register(
                Component.For<IDocumentStore>().Instance(documentStore).LifestyleSingleton(),
                Component.For<IAsyncDocumentSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IDocumentStore>().OpenAsyncSession()).LifestyleScoped());
        }

        private DocumentStore CreateRavenDocumentStore()
        {
            if (Config.RunRavenInEmbeddedMode)
            {
                var config = new RavenConfiguration
                {
                    Port = Config.RavenHttpServerPort,
                    MaxSecondsForTaskToWaitForDatabaseToLoad = Config.MaxSecondsToWaitForDatabaseToLoad,
                    Settings =
                {
                    ["Raven/WorkingDir"] = "~/Raven/",
                    ["Raven/StorageEngine"] = "voron"
                },
                };

                var server = new RavenDbServer(config);
                server.Initialize();

                server.DocumentStore.DefaultDatabase = Config.RavenName;
                server.DocumentStore.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(server.DocumentStore.DefaultDatabase);

                if (Config.EnableRavenHttpServer)
                {
                    server.EnableHttpServer();
                }

                return server.DocumentStore;
            }
            else
            {
                var documentStore = new DocumentStore();
                documentStore.ParseConnectionString(Config.RavenConnectionString);

                documentStore.Initialize();

                return documentStore;
            }
        }

        private void CustomizeRavenDocumentStore(DocumentStore store)
        {
            store.Conventions.RegisterAsyncIdConvention<AuthenticationData>((databaseName, commands, obj) =>
                Task.FromResult(AuthenticationData.CreateId(obj.ForUserId)));
            store.Conventions.RegisterAsyncIdConvention<UserSummoners>((databaseName, commands, obj) =>
                Task.FromResult(UserSummoners.CreateId(obj.ForUserId)));
            store.Conventions.RegisterAsyncIdConvention<Summoner>((databaseName, commands, obj) =>
                Task.FromResult(Summoner.CreateId(obj.RiotSummonerId, obj.Region)));
        }
    }
}