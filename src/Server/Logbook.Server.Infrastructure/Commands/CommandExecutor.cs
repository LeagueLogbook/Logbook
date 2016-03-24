using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using JetBrains.Annotations;
using Logbook.Server.Contracts.Commands;
using Logbook.Shared;
using Logbook.Shared.Extensions;
using NHibernate;

namespace Logbook.Server.Infrastructure.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IWindsorContainer _container;

        public CommandExecutor(IWindsorContainer container)
        {
            Guard.NotNull(container, nameof(container));

            this._container = container;
        }

        public async Task<T> Batch<T>(Func<ICommandScope, Task<T>> batchAction)
        {
            Guard.NotNull(batchAction, nameof(batchAction));

            using (this._container.BeginScope())
            {
                var session = this._container.Resolve<ISession>();
                var scope = this._container.Resolve<ICommandScope>();

                var transaction = session.BeginTransaction();

                try
                {
                    T result = await batchAction(scope);

                    transaction.Commit();

                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this._container.Release(session);
                    this._container.Release(scope);

                    transaction.Dispose();
                }
            }
        }
    }
}