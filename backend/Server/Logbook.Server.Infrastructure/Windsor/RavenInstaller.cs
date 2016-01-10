using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Shared.Entities.Authentication;
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
            RavenDbServer server = this.CreateRavenDbServer();

            container.Register(
                Component.For<IDocumentStore>().Instance(server.DocumentStore).LifestyleSingleton(),
                Component.For<IAsyncDocumentSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IDocumentStore>().OpenAsyncSession()).LifestyleScoped(),
                Component.For<IFilesStore>().Instance(server.FilesStore).LifestyleSingleton(),
                Component.For<IAsyncFilesSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IFilesStore>().OpenAsyncSession()).LifestyleScoped());
        }

        private RavenDbServer CreateRavenDbServer()
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

            server.FilesStore.DefaultFileSystem = Config.RavenName;
            server.FilesStore.AsyncFilesCommands.Admin.EnsureFileSystemExistsAsync(server.DocumentStore.DefaultDatabase).Wait();

            if (Config.EnableRavenHttpServer)
            {
                server.EnableHttpServer();
            }

            this.CustomizeRavenDocumentStore(server.DocumentStore);

            return server;
        }

        private void CustomizeRavenDocumentStore(DocumentStore store)
        {
            store.Conventions.RegisterAsyncIdConvention<AuthenticationData>((databaseName, commands, obj) =>
                Task.FromResult(AuthenticationData.CreateId(obj.ForUserId)));
        }
    }
}