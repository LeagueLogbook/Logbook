using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Riot;
using Logbook.Shared.Entities.Summoners;
using RiotSharp;
using Region = Logbook.Shared.Entities.Summoners.Region;

namespace Logbook.Server.Infrastructure.Riot
{
    public class LeagueService : ILeagueService
    {
        private readonly RiotApi _api;
        private readonly Dictionary<Region, RiotSharp.Region> _regionMapping;

        public LeagueService()
        {
            this._api = RiotApi.GetInstance(Config.RiotApiKey, Config.RiotApiRateLimitPer10Seconds, Config.RiotApiRateLimitPer10Minutes);

            this._regionMapping = new Dictionary<Region, RiotSharp.Region>
            {
                [Region.Br] = RiotSharp.Region.br,
                [Region.Eune] = RiotSharp.Region.eune,
                [Region.Euw] = RiotSharp.Region.euw,
                [Region.Na] = RiotSharp.Region.na,
                [Region.Kr] = RiotSharp.Region.kr,
                [Region.Lan] = RiotSharp.Region.lan,
                [Region.Las] = RiotSharp.Region.las,
                [Region.Oce] = RiotSharp.Region.oce,
                [Region.Ru] = RiotSharp.Region.ru,
                [Region.Tr] = RiotSharp.Region.tr,
                [Region.Global] = RiotSharp.Region.global
            };
        }

        public async Task<Summoner> GetSummonerAsync(Region region, string name)
        {
            try
            {
                var result = await this._api.GetSummonerAsync(this.ConvertRegion(region), name);

                if (result == null)
                    return null;

                return this.ConvertSummoner(result);
            }
            catch (RiotSharpException e) when (e.Message.StartsWith("404"))
            {
                return null;
            }
        }

        private Summoner ConvertSummoner(RiotSharp.SummonerEndpoint.Summoner summoner)
        {
            return new Summoner
            {
                RiotSummonerId = summoner.Id,
                Name = summoner.Name,
                Region = this.ConvertRegion(summoner.Region),
                Level = (int)summoner.Level,
                ProfileIconUri = $"http://ddragon.leagueoflegends.com/cdn/6.1.1/img/profileicon/{summoner.ProfileIconId}.png"
            };
        }

        private RiotSharp.Region ConvertRegion(Region region)
        {
            return this._regionMapping[region];
        }

        private Region ConvertRegion(RiotSharp.Region region)
        {
            return this._regionMapping.First(f => f.Value == region).Key;
        }
    }
}