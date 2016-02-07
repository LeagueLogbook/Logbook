using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Infrastructure.Api.Filter;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.ControllerModels;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Extensions;
using Logbook.Shared.Models;
using Logbook.Shared.Models.Summoners;

namespace Logbook.Server.Infrastructure.Api.Controllers
{
    public class SummonersController : BaseController
    {
        public SummonersController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
        }

        [HttpGet]
        [Route("Summoners")]
        [LogbookAuthentication]
        public async Task<HttpResponseMessage> GetSummoners()
        {
            var result = await this.CommandExecutor.Batch(async scope =>
            {
                var summoners = await scope.Execute(new GetSummonersCommand(this.CurrentUserId));
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
            throw new NotImplementedException();
            //if (data?.SummonerName == null)
            //    throw new DataMissingException();

            //var summoners = await this.CommandExecutor
            //    .Batch(async scope =>
            //    {
            //        await scope.Execute(new AddSummonerCommand(this.CurrentUserId, data.Region, data.SummonerName));
            //        return await scope.Execute(new GetSummonerModelsCommand(this.CurrentUserId));
            //    })
            //    .WithCurrentCulture();

            //return this.Request.GetMessageWithObject(HttpStatusCode.Found, summoners);
        }


        [HttpDelete]
        [Route("Summoners")]
        [LogbookAuthentication]
        public async Task<HttpResponseMessage> DeleteSummoner(DeleteSummonerData data)
        {
            throw new NotImplementedException();
            //if (data?.SummonerId == null)
            //    throw new DataMissingException();

            //var summoners = await this.CommandExecutor
            //    .Batch(async scope =>
            //    {
            //        await scope.Execute(new RemoveSummonerCommand(this.CurrentUserId, data.Region, data.SummonerId));
            //        return await scope.Execute(new GetSummonerModelsCommand(this.CurrentUserId));
            //    })
            //    .WithCurrentCulture();

            //return this.Request.GetMessageWithObject(HttpStatusCode.OK, summoners);
        }
    }
}