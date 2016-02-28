using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Riot;
using Logbook.Server.Infrastructure.Configuration;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.Games;
using Logbook.Shared.Models.MatchHistory;
using RiotSharp;
using RiotSharp.MatchEndpoint;
using RiotSharp.StaticDataEndpoint;
using BannedChampion = Logbook.Shared.Models.Games.BannedChampion;
using GameMode = Logbook.Shared.Models.Games.GameMode;
using GameType = Logbook.Shared.Models.Games.GameType;
using Lane = Logbook.Shared.Entities.Summoners.Lane;
using MapType = Logbook.Shared.Models.Games.MapType;
using Participant = Logbook.Shared.Models.Games.Participant;
using Region = Logbook.Shared.Entities.Summoners.Region;
using Role = Logbook.Shared.Entities.Summoners.Role;
using Team = Logbook.Shared.Models.Games.Team;

namespace Logbook.Server.Infrastructure.Riot
{
    public class LeagueService : ILeagueService
    {
        #region Fields
        private readonly RiotApi _api;
        private readonly StaticRiotApi _staticApi;
        private readonly Dictionary<Region, RiotSharp.Region> _regionMapping;
        private readonly Dictionary<Region, Platform> _regionToPlatformMapping;
        private readonly Dictionary<GameQueueType, RiotSharp.CurrentGameEndpoint.Converters.GameQueueType> _gameQueueTypeMapping;
        private readonly Dictionary<GameMode, RiotSharp.GameMode> _gameModeMapping;
        private readonly Dictionary<MapType, RiotSharp.MapType> _mapTypeMapping;
        private readonly Dictionary<GameType, RiotSharp.GameType> _gameTypeMapping;
        private readonly Dictionary<RiotSharp.MatchEndpoint.Lane, Lane> _laneMapping;
        private readonly Dictionary<RiotSharp.MatchEndpoint.Role, Role> _roleMapping;
        #endregion

        #region Constructors
        public LeagueService()
        {
            this._api = RiotApi.GetInstance(Config.Riot.RiotApiKey, Config.Riot.RiotApiRateLimitPer10Seconds, Config.Riot.RiotApiRateLimitPer10Minutes);
            this._staticApi = StaticRiotApi.GetInstance(Config.Riot.RiotApiKey);

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
            };
            this._regionToPlatformMapping = new Dictionary<Region, Platform>
            {
                [Region.Br] = Platform.BR1,
                [Region.Eune] = Platform.EUN1,
                [Region.Euw] = Platform.EUW1,
                [Region.Na] = Platform.NA1,
                [Region.Kr] = Platform.KR,
                [Region.Lan] = Platform.LA1,
                [Region.Las] = Platform.LA2,
                [Region.Oce] = Platform.OC1,
                [Region.Ru] = Platform.RU,
                [Region.Tr] = Platform.TR1,
            };
            this._gameQueueTypeMapping = new Dictionary<GameQueueType, RiotSharp.CurrentGameEndpoint.Converters.GameQueueType>
            {
                [GameQueueType.Custom] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Custom,
                [GameQueueType.Normal5x5Blind] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Normal5x5Blind,
                [GameQueueType.RankedSolo5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.RankedSolo5x5,
                [GameQueueType.RankedPremade5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.RankedPremade5x5,
                [GameQueueType.Bot5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Bot5x5,
                [GameQueueType.Normal3x3] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Normal3x3,
                [GameQueueType.RankedPremade3x3] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.RankedPremade3x3,
                [GameQueueType.Normal5x5Draft] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Normal5x5Draft,
                [GameQueueType.Odin5x5Blind] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Odin5x5Blind,
                [GameQueueType.Odin5x5Draft] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Odin5x5Draft,
                [GameQueueType.BotOdin5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.BotOdin5x5,
                [GameQueueType.Bot5x5Intro] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Bot5x5Intro,
                [GameQueueType.Bot5x5Beginner] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Bot5x5Beginner,
                [GameQueueType.Bot5x5Intermediate] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Bot5x5Intermediate,
                [GameQueueType.RankedTeam3x3] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.RankedTeam3x3,
                [GameQueueType.RankedTeam5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.RankedTeam5x5,
                [GameQueueType.BotTt3x3] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.BotTt3x3,
                [GameQueueType.GroupFinder5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.GroupFinder5x5,
                [GameQueueType.Aram5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Aram5x5,
                [GameQueueType.Oneforall5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Oneforall5x5,
                [GameQueueType.Firstblood1x1] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Firstblood1x1,
                [GameQueueType.Firstblood2x2] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Firstblood2x2,
                [GameQueueType.Sr6x6] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Sr6x6,
                [GameQueueType.Urf5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Urf5x5,
                [GameQueueType.BotUrf5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.BotUrf5x5,
                [GameQueueType.NightmareBot5x5Rank1] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.NightmareBot5x5Rank1,
                [GameQueueType.NightmareBot5x5Rank2] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.NightmareBot5x5Rank2,
                [GameQueueType.NightmareBot5x5Rank5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.NightmareBot5x5Rank5,
                [GameQueueType.Ascension5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Ascension5x5,
                [GameQueueType.Hexakill] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Hexakill,
                [GameQueueType.BilgewaterAram5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.BilgewaterAram5x5,
                [GameQueueType.KingPoro5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.KingPoro5x5,
                [GameQueueType.Bilgewater5x5] = RiotSharp.CurrentGameEndpoint.Converters.GameQueueType.Bilgewater5x5,
            };
            this._gameModeMapping = new Dictionary<GameMode, RiotSharp.GameMode>
            {
                [GameMode.Classic] = RiotSharp.GameMode.Classic,
                [GameMode.Dominion] = RiotSharp.GameMode.Dominion,
                [GameMode.Aram] = RiotSharp.GameMode.Aram,
                [GameMode.Tutorial] = RiotSharp.GameMode.Tutorial,
                [GameMode.OneForAll] = RiotSharp.GameMode.OneForAll,
                [GameMode.FirstBlood] = RiotSharp.GameMode.FirstBlood,
                [GameMode.Ascension] = RiotSharp.GameMode.Ascension,
                [GameMode.Intro] = RiotSharp.GameMode.Intro,
                [GameMode.KingPoro] = RiotSharp.GameMode.KingPoro,
            };
            this._mapTypeMapping = new Dictionary<MapType, RiotSharp.MapType>
            {
                [MapType.SummonersRiftSummerVariant] = RiotSharp.MapType.SummonersRiftSummerVariant,
                [MapType.SummonersRiftAutumnVariant] = RiotSharp.MapType.SummonersRiftAutumnVariant,
                [MapType.TheProvingGrounds] = RiotSharp.MapType.TheProvingGrounds,
                [MapType.TwistedTreelineOriginal] = RiotSharp.MapType.TwistedTreelineOriginal,
                [MapType.TheCrystalScar] = RiotSharp.MapType.TheCrystalScar,
                [MapType.TwistedTreelineCurrent] = RiotSharp.MapType.TwistedTreelineCurrent,
                [MapType.SummonersRift] = RiotSharp.MapType.SummonersRift,
                [MapType.HowlingAbyss] = RiotSharp.MapType.HowlingAbyss,
            };
            this._gameTypeMapping = new Dictionary<GameType, RiotSharp.GameType>
            {
                [GameType.Custom] = RiotSharp.GameType.CustomGame,
                [GameType.Matched] = RiotSharp.GameType.MatchedGame,
                [GameType.Tutorial] = RiotSharp.GameType.TutorialGame
            };
            this._laneMapping = new Dictionary<RiotSharp.MatchEndpoint.Lane, Lane>
            {
                [RiotSharp.MatchEndpoint.Lane.Bot] = Lane.Bot,
                [RiotSharp.MatchEndpoint.Lane.Bottom] = Lane.Bot,
                [RiotSharp.MatchEndpoint.Lane.Jungle] = Lane.Jungle,
                [RiotSharp.MatchEndpoint.Lane.Mid] = Lane.Mid,
                [RiotSharp.MatchEndpoint.Lane.Middle] = Lane.Mid,
                [RiotSharp.MatchEndpoint.Lane.Top] = Lane.Top,
            };
            this._roleMapping = new Dictionary<RiotSharp.MatchEndpoint.Role, Role>
            {
                [RiotSharp.MatchEndpoint.Role.Duo] = Role.Duo,
                [RiotSharp.MatchEndpoint.Role.DuoCarry] = Role.DuoCarry,
                [RiotSharp.MatchEndpoint.Role.DuoSupport] = Role.DuoSupport,
                [RiotSharp.MatchEndpoint.Role.None] = Role.None,
                [RiotSharp.MatchEndpoint.Role.Solo] = Role.Solo,
            };
        }
        #endregion

        #region Implementation of ILeagueService

        public async Task<Summoner> GetSummonerAsync(Region region, long riotSummonerId)
        {
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotZeroOrNegative(riotSummonerId, nameof(riotSummonerId));

            try
            {
                var result = await this._api.GetSummonerAsync(this.ConvertRegion(region), (int)riotSummonerId);

                if (result == null)
                    return null;

                return this.ConvertSummoner(result);
            }
            catch (RiotSharpException e) when (e.Message.StartsWith("404"))
            {
                return null;
            }
        }

        public async Task<Summoner> GetSummonerAsync(Region region, string name)
        {
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotNullOrWhiteSpace(name, nameof(name));

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

        public async Task<IList<Summoner>> GetSummonersAsync(Region region, IList<long> riotSummonerIds)
        {
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotNullOrEmpty(riotSummonerIds, nameof(riotSummonerIds));

            var summoners = await this._api.GetSummonersAsync(this.ConvertRegion(region), riotSummonerIds.Select(f => (int)f).ToList());
            return summoners.Select(this.ConvertSummoner).ToList();
        }

        public async Task<CurrentGame> GetCurrentGameAsync(Region region, long riotSummonerId)
        {
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotZeroOrNegative(riotSummonerId, nameof(riotSummonerId));

            try
            {
                var result = await this._api.GetCurrentGameAsync(this.ConvertRegionToPlatform(region), riotSummonerId);

                if (result == null)
                    return null;

                return this.ConvertCurrentGame(result, region);
            }
            catch (RiotSharpException e) when (e.Message.StartsWith("404"))
            {
                return null;
            }
        }

        public async Task<List<long>>  GetMatchHistory(Region region, long summonerId, DateTime? latestCheckedMatchTimeStamp)
        {
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotZeroOrNegative(summonerId, nameof(summonerId));

            latestCheckedMatchTimeStamp = latestCheckedMatchTimeStamp?.AddSeconds(1);

            var matchList = await this._api.GetMatchListAsync(
                this.ConvertRegion(region),
                summonerId,
                seasons: new List<Season>() {Season.Season2016},
                beginTime: latestCheckedMatchTimeStamp);

            if (matchList.Matches == null)
                return new List<long>();

            return matchList.Matches
                .Select(f => f.MatchID)
                .OrderBy(f => f)
                .ToList();
        }

        public async Task<PlayedMatch> GetMatch(Region region, long matchId)
        {
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotZeroOrNegative(matchId , nameof(matchId));

            MatchDetail match = await this._api.GetMatchAsync(this.ConvertRegion(region), matchId, includeTimeline: true);
            return this.ConvertPlayedMatch(match);
        }
        #endregion

        #region StaticData Cache
        private readonly ConcurrentDictionary<Region, ChampionListStatic> _cachedChampions = new ConcurrentDictionary<Region, ChampionListStatic>();
        private readonly ConcurrentDictionary<Region, SummonerSpellListStatic> _cachedSummonerSpells = new ConcurrentDictionary<Region, SummonerSpellListStatic>();

        private ChampionListStatic GetChampionList(Region region)
        {
            Guard.NotInvalidEnum(region, nameof(region));

            return this._cachedChampions.GetOrAdd(region, f => this._staticApi.GetChampions(this.ConvertRegion(f), ChampionData.info));
        }

        private SummonerSpellListStatic GetSummonerSpells(Region region)
        {
            Guard.NotInvalidEnum(region, nameof(region));

            return this._cachedSummonerSpells.GetOrAdd(region, f => this._staticApi.GetSummonerSpells(this.ConvertRegion(f), SummonerSpellData.image));
        }
        #endregion
        
        #region Class Conversion
        private CurrentGame ConvertCurrentGame(RiotSharp.CurrentGameEndpoint.CurrentGame currentGame, Region region)
        {
            Guard.NotNull(currentGame, nameof(currentGame));
            Guard.NotInvalidEnum(region, nameof(region));

            return new CurrentGame
            {
                GameId = currentGame.GameId,
                GameQueueType = this.ConvertGameQueueType(currentGame.GameQueueType),
                GameType = this.ConvertGameType(currentGame.GameType),
                GameStartTime = currentGame.GameStartTime,
                BlueTeam = new Team
                {
                    Participants = currentGame.Participants
                        .Where(f => f.TeamId == 100)
                        .Select(f => this.ConvertParticipant(f, region))
                        .ToList(),
                    BannedChampions = currentGame.BannedChampions
                        .Where(f => f.TeamId == 100)
                        .Select(f => this.ConvertBannedChampion(f, region))
                        .ToList()
                },
                PurpleTeam = new Team
                {
                    Participants = currentGame.Participants
                        .Where(f => f.TeamId == 200)
                        .Select(f => this.ConvertParticipant(f, region))
                        .ToList(),
                    BannedChampions = currentGame.BannedChampions
                        .Where(f => f.TeamId == 200)
                        .Select(f => this.ConvertBannedChampion(f, region))
                        .ToList()
                },
                GameMode = this.ConvertGameMode(currentGame.GameMode),
                MapType = this.ConvertMapType(currentGame.MapType),
                Region = region
            };
        }

        private BannedChampion ConvertBannedChampion(RiotSharp.CurrentGameEndpoint.BannedChampion bannedChampion, Region region)
        {
            Guard.NotNull(bannedChampion, nameof(bannedChampion));
            Guard.NotInvalidEnum(region, nameof(region));

            return new BannedChampion
            {
                ChampionId = bannedChampion.ChampionId,
                ChampionName = this.GetChampionList(region).Champions.FirstOrDefault(f => f.Value.Id == bannedChampion.ChampionId).Value?.Name,
                PickTurn = bannedChampion.PickTurn
            };
        }

        private Participant ConvertParticipant(RiotSharp.CurrentGameEndpoint.Participant participant, Region region)
        {
            Guard.NotNull(participant, nameof(participant));
            Guard.NotInvalidEnum(region, nameof(region));

            return new Participant
            {
                IsBot = participant.Bot,
                SummonerId = participant.SummonerId,
                ProfileIconId = (int)participant.ProfileIconId,
                SummonerName = participant.SummonerName,
                ChampionId = participant.ChampionId,
                ChampionName = this.GetChampionList(region).Champions.FirstOrDefault(f => f.Value.Id == participant.ChampionId).Value?.Name,
                SummonerSpell1Id = participant.SummonuerSpell1,
                SummonerSpell1Name = this.GetSummonerSpells(region).SummonerSpells.FirstOrDefault(f => f.Value.Id == participant.SummonuerSpell1).Value?.Name,
                SummonerSpell2Id = participant.SummonerSpell2,
                SummonerSpell2Name = this.GetSummonerSpells(region).SummonerSpells.FirstOrDefault(f => f.Value.Id == participant.SummonerSpell2).Value?.Name,
            };
        }

        private Summoner ConvertSummoner(RiotSharp.SummonerEndpoint.Summoner summoner)
        {
            Guard.NotNull(summoner, nameof(summoner));

            return new Summoner
            {
                RiotSummonerId = summoner.Id,
                Name = summoner.Name,
                Region = this.ConvertRegion(summoner.Region),
                Level = (int)summoner.Level,
                ProfileIconId = summoner.ProfileIconId,
            };
        }

        private PlayedMatch ConvertPlayedMatch(MatchDetail match)
        {
            Func<RiotSharp.MatchEndpoint.Participant, Shared.Models.MatchHistory.Participant> convertParticipant = f => new Shared.Models.MatchHistory.Participant
            {
                SummonerId = match.ParticipantIdentities.First(d => d.ParticipantId == f.ParticipantId).Player.SummonerId,
                Lane = this.ConvertLane(f.Timeline.Lane),
                Role = this.ConvertRole(f.Timeline.Role),
                ChampionId = f.ChampionId,
                Assists = f.Stats.Assists,
                Kills = f.Stats.Kills,
                Deaths = f.Stats.Deaths,
                Creeps = f.Stats.MinionsKilled,
                DestroyedWards = f.Stats.WardsKilled,
                PlacedWards = f.Stats.WardsPlaced,
            };
            return new PlayedMatch
            {
                Duration = match.MatchDuration,
                CreationDate = match.MatchCreation,
                MatchId = match.MatchId,
                PurpleTeam = new Shared.Models.MatchHistory.Team
                {
                    Winner = match.Teams.First(f => f.TeamId == 200).Winner,
                    Participants = match.Participants
                        .Where(f => f.TeamId == 200)
                        .Select(convertParticipant)
                        .ToList()
                },
                BlueTeam = new Shared.Models.MatchHistory.Team
                {
                    Winner = match.Teams.First(f => f.TeamId == 100).Winner,
                    Participants = match.Participants
                        .Where(f => f.TeamId == 100)
                        .Select(convertParticipant)
                        .ToList()
                },
                Region = this.ConvertRegion(match.Region)
            };
        }
        #endregion

        #region Enum Conversion
        private MapType ConvertMapType(RiotSharp.MapType mapType)
        {
            Guard.NotInvalidEnum(mapType, nameof(mapType));

            return this._mapTypeMapping.First(f => f.Value == mapType).Key;
        }

        private GameMode ConvertGameMode(RiotSharp.GameMode gameMode)
        {
            Guard.NotInvalidEnum(gameMode, nameof(gameMode));

            return this._gameModeMapping.First(f => f.Value == gameMode).Key;
        }

        private GameType ConvertGameType(RiotSharp.GameType gameType)
        {
            Guard.NotInvalidEnum(gameType, nameof(gameType));

            return this._gameTypeMapping.First(f => f.Value == gameType).Key;
        }

        private GameQueueType ConvertGameQueueType(RiotSharp.CurrentGameEndpoint.Converters.GameQueueType gameQueueType)
        {
            Guard.NotInvalidEnum(gameQueueType, nameof(gameQueueType));

            return this._gameQueueTypeMapping.First(f => f.Value == gameQueueType).Key;
        }

        private Platform ConvertRegionToPlatform(Region region)
        {
            Guard.NotInvalidEnum(region, nameof(region));

            return this._regionToPlatformMapping[region];
        }

        private RiotSharp.Region ConvertRegion(Region region)
        {
            Guard.NotInvalidEnum(region, nameof(region));

            return this._regionMapping[region];
        }

        private Region ConvertRegion(RiotSharp.Region region)
        {
            Guard.NotInvalidEnum(region, nameof(region));

            return this._regionMapping.First(f => f.Value == region).Key;
        }

        private Role ConvertRole(RiotSharp.MatchEndpoint.Role role)
        {
            return this._roleMapping[role];
        }

        private Lane ConvertLane(RiotSharp.MatchEndpoint.Lane lane)
        {
            return this._laneMapping[lane];
        }
        #endregion
    }
}