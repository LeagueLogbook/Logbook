using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Server.Infrastructure.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace Logbook.Server.Infrastructure.Windsor
{
    public class AzureStorageInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<CloudQueueClient>().UsingFactoryMethod((kernel, context) => this.CreateQueueClient()).LifestyleSingleton(),
                Component.For<CloudTableClient>().UsingFactoryMethod((kernel, context) => this.CreateTableClient()).LifestyleSingleton());
        }

        private CloudStorageAccount GetStorageAccount()
        {
            return CloudStorageAccount.Parse(Config.Azure.AzureStorageConnectionString);
        }

        private CloudQueueClient CreateQueueClient()
        {
            return this.GetStorageAccount().CreateCloudQueueClient();
        }

        private CloudTableClient CreateTableClient()
        {
            return this.GetStorageAccount().CreateCloudTableClient();
        }
    }
}