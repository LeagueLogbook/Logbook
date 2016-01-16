using System.Threading.Tasks;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Shared.Entities.Summoners;
using Raven.Client;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class GetSummonersCommandHandler : ICommandHandler<GetSummonersCommand, UserSummoners>
    {
        private readonly IAsyncDocumentSession _documentSession;

        public GetSummonersCommandHandler(IAsyncDocumentSession documentSession)
        {
            Guard.AgainstNullArgument(nameof(documentSession), documentSession);

            this._documentSession = documentSession;
        }

        public async Task<UserSummoners> Execute(GetSummonersCommand command, ICommandScope scope)
        {
            var id = UserSummoners.CreateId(command.UserId);

            var result = await this._documentSession.LoadAsync<UserSummoners>(id);

            if (result == null)
            {
                result = new UserSummoners
                {
                    ForUserId = command.UserId
                };

                await this._documentSession.StoreAsync(result);
            }

            return result;
        }
    }
}