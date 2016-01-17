using System.Collections.Generic;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Contracts.Mapping;
using Logbook.Server.Infrastructure.Mapping;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models;
using Raven.Client;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class GetSummonerModelsCommandHandler : ICommandHandler<GetSummonerModelsCommand, IList<SummonerModel>>
    {
        private readonly IAsyncDocumentSession _documentSession;
        private readonly IMapper<Summoner, SummonerModel> _summonerMapper;

        public GetSummonerModelsCommandHandler(IAsyncDocumentSession documentSession, IMapper<Summoner, SummonerModel> summonerMapper)
        {
            this._documentSession = documentSession;
            this._summonerMapper = summonerMapper;
        }

        public async Task<IList<SummonerModel>> Execute(GetSummonerModelsCommand command, ICommandScope scope)
        {
            var userSummoners = await scope.Execute(new GetSummonersCommand(command.UserId));
            var summoners = await this._documentSession.LoadAsync<Summoner>(userSummoners.SummonerIds);

            return await this._summonerMapper.MapListAsync(summoners);
        }
    }
}