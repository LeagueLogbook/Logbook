using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Admin;
using Raven.Client;
using Raven.Client.Indexes;

namespace Logbook.Server.Infrastructure.Commands.Admin
{
    public class CreateIndexesCommandHandler : ICommandHandler<CreateIndexesCommand, object>
    {
        #region Fields
        private readonly IDocumentStore _documentStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIndexesCommandHandler"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        public CreateIndexesCommandHandler([NotNull]IDocumentStore documentStore)
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
        public async Task<object> Execute(CreateIndexesCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument(nameof(command), command);
            Guard.AgainstNullArgument(nameof(scope), scope);

            await IndexCreation.CreateIndexesAsync(this.GetType().Assembly, this._documentStore);
            return new object();
        }
        #endregion
    }
}