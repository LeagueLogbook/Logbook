using System;
using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Admin;
using Raven.Client;
using Raven.Client.Indexes;

namespace Logbook.Server.Infrastructure.Commands.Admin
{
    public class ResetIndexesCommandHandler : ICommandHandler<ResetIndexesCommand, object>
    {
        #region Fields
        private readonly IDocumentStore _documentStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ResetIndexesCommandHandler"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        public ResetIndexesCommandHandler([NotNull]IDocumentStore documentStore)
        {
            Guard.AgainstNullArgument(nameof(documentStore), documentStore);

            this._documentStore = documentStore;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified <paramref name="command" />.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public async Task<object> Execute(ResetIndexesCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument(nameof(command), command);
            Guard.AgainstNullArgument(nameof(scope), scope);
            
            var container = new CompositionContainer(new AssemblyCatalog(this.GetType().Assembly));
            foreach (var index in container.GetExportedValues<AbstractIndexCreationTask>())
            {
                await this._documentStore.AsyncDatabaseCommands.ResetIndexAsync(index.IndexName);
            }

            return new object();
        }
        #endregion
    }
}