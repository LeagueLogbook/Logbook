using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Logbook.Server.Infrastructure.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Logbook.Server.Infrastructure.Windsor
{
    public class AzureStorageInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<CloudQueueClient>().UsingFactoryMethod((kernel, context) => this.CreateQueueClient()).LifestyleSingleton());
        }

        private CloudStorageAccount GetStorageAccount()
        {
            return CloudStorageAccount.Parse(Config.Azure.AzureStorageConnectionString);
        }

        private CloudQueueClient CreateQueueClient()
        {
            return this.GetStorageAccount().CreateCloudQueueClient();
        }
    }
}