using System;
using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Admin;
using Logbook.Shared.Results;
using Raven.Client;
using Raven.Client.Indexes;

namespace Logbook.Server.Infrastructure.Commands.Admin
{
    public class ResetIndexesCommandHandler : ICommandHandler<ResetIndexesCommand, object>
    {
        private readonly IDocumentStore _documentStore;

        public ResetIndexesCommandHandler([NotNull]IDocumentStore documentStore)
        {
            Guard.AgainstNullArgument(nameof(documentStore), documentStore);

            this._documentStore = documentStore;
        }

        public Task<Result<object>> Execute(ResetIndexesCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument(nameof(command), command);
            Guard.AgainstNullArgument(nameof(scope), scope);

            return Result.CreateAsync(async () =>
            {
                var container = new CompositionContainer(new AssemblyCatalog(this.GetType().Assembly));
                foreach (var index in container.GetExportedValues<AbstractIndexCreationTask>())
                {
                    await this._documentStore.AsyncDatabaseCommands.ResetIndexAsync(index.IndexName);
                }

                return new object();
            });
        }
    }
}