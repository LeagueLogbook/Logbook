using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Shared.Extensions;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Logbook.Server.Infrastructure.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IWindsorContainer _container;

        public CommandExecutor([NotNull]IWindsorContainer container)
        {
            Guard.AgainstNullArgument(nameof(container), container);

            this._container = container;
        }

        public async Task<T> Batch<T>(Func<ICommandScope, Task<T>> batchAction)
        {
            using (this._container.BeginScope())
            {
                var documentSession = this._container.Resolve<IAsyncDocumentSession>();
                var filesSession = this._container.Resolve<IAsyncFilesSession>();

                var scope = this._container.Resolve<ICommandScope>();

                T result = await batchAction(scope).WithCurrentCulture();

                await documentSession.SaveChangesAsync();
                await filesSession.SaveChangesAsync();

                this._container.Release(documentSession);
                this._container.Release(filesSession);

                return result;
            }
        }
    }
}