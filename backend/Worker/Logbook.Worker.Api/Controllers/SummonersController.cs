using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.Summoners;
using Logbook.Worker.Api.Extensions;
using Logbook.Worker.Api.Filter;
using Logbook.Worker.Api.Models;

namespace Logbook.Worker.Api.Controllers
{
    public class SummonersController : BaseController
    {
        public SummonersController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
            Guard.NotNull(commandExecutor, nameof(commandExecutor));
        }

        [HttpGet]
        [Route("Summoners")]
        [LogbookAuthentication]
        public async Task<HttpResponseMessage> GetSummoners()
        {
            var result = await this.CommandExecutor.Batch(async scope =>
            {
                var summoners = await scope.Execute(new GetUserSummonersCommand(this.CurrentUserId));
                var models = await scope.MapList<Summoner, SummonerModel>(summoners);

                return models;
            });

            return this.Request.GetMessageWithObject(HttpStatusCode.OK, result);
        }

        [HttpPatch]
        [Route("Summoners")]
        [LogbookAuthentication]
        public async Task<HttpResponseMessage> AddSummoner(AddSummonerData data)
        {
            if (data?.SummonerName == null)
                throw new DataMissingException();

            var result = await this.CommandExecutor.Batch(async scope =>
            {
                await scope.Execute(new AddSummonerCommand(this.CurrentUserId, data.Region, data.SummonerName));
                var summoners = await scope.Execute(new GetUserSummonersCommand(this.CurrentUserId));
                var models = await scope.MapList<Summoner, SummonerModel>(summoners);

                return models;
            });

            return this.Request.GetMessageWithObject(HttpStatusCode.Found, result);
        }


        [HttpDelete]
        [Route("Summoners")]
        [LogbookAuthentication]
        public async Task<HttpResponseMessage> DeleteSummoner(DeleteSummonerData data)
        {
            if (data?.SummonerId == null)
                throw new DataMissingException();

            var result = await this.CommandExecutor.Batch(async scope =>
            {
                await scope.Execute(new RemoveSummonerCommand(this.CurrentUserId, data.Region, data.SummonerId));
                var summoners = await scope.Execute(new GetUserSummonersCommand(this.CurrentUserId));
                var models = await scope.MapList<Summoner, SummonerModel>(summoners);

                return models;
            });

            return this.Request.GetMessageWithObject(HttpStatusCode.OK, result);
        }
    }
}