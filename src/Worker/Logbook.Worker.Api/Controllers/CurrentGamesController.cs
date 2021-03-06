using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.CurrentGames;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.Games;
using Logbook.Shared.Models.Summoners;
using Logbook.Worker.Api.Extensions;
using Logbook.Worker.Api.Filter;

namespace Logbook.Worker.Api.Controllers
{
    public class CurrentGamesController : BaseController
    {
        public CurrentGamesController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
            Guard.NotNull(commandExecutor, nameof(commandExecutor));
        }

        [HttpGet]
        [Route("CurrentGames")]
        [LogbookAuthentication]
        public async Task<HttpResponseMessage> GetCurrentGames()
        {
            var result = await this.CommandExecutor.Batch(async scope =>
            {
                var games = new Dictionary<SummonerModel, CurrentGame>();
                
                var summoners = await scope.Execute(new GetUserSummonersCommand(this.CurrentUserId));

                foreach (var summoner in summoners)
                {
                    var currentGame = await scope.Execute(new GetCurrentGameCommand(summoner.Region, summoner.RiotSummonerId));
                    if (currentGame != null)
                    {
                        var summonerModel = await scope.Map<Summoner, SummonerModel>(summoner);
                        games.Add(summonerModel, currentGame);
                    }
                }

                return games;
            });

            if (result.Any() == false)
                throw new LogbookException("Noone is currently ingame.");

            return this.Request.GetMessageWithObject(HttpStatusCode.Found, result.Select(f => new { Summoner = f.Key, CurrentGame = f.Value }).ToList());
        }
    }
}