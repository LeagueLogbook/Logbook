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
    public class CreateIndexesCommandHandler : ICommandHandler<CreateIndexesCommand, object>
    {
        private readonly IDocumentStore _documentStore;

        public CreateIndexesCommandHandler([NotNull]IDocumentStore documentStore)
        {
            Guard.AgainstNullArgument(nameof(documentStore), documentStore);

            this._documentStore = documentStore;
        }

        public Task<Result<object>> Execute(CreateIndexesCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument(nameof(command), command);
            Guard.AgainstNullArgument(nameof(scope), scope);

            return Result.CreateAsync(async () =>
            {
                await IndexCreation.CreateIndexesAsync(this.GetType().Assembly, this._documentStore);
                return new object();
            });
        }
    }
}