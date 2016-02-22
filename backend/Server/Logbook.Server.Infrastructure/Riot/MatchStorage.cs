using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Riot;
using Logbook.Server.Infrastructure.Configuration;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.MatchHistory;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Logbook.Server.Infrastructure.Riot
{
    public class MatchStorage : IMatchStorage
    {
        private readonly CloudTableClient _client;
        private CloudTable _table;

        public MatchStorage(CloudTableClient client)
        {
            Guard.NotNull(client, nameof(client));

            this._client = client;
        }

        public async Task SaveMatchAsync(PlayedMatch match)
        {
            Guard.NotNull(match, nameof(match));

            var entity = new PlayedMatchTableEntity(match.Region.ToString(), match.MatchId.ToString())
            {
                PlayedMatchAsJson = JsonConvert.SerializeObject(match)
            };

            var table = await this.GetTableAsync();
            await table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

        public async Task<IList<PlayedMatch>> GetMatchesAsync(Region region, IList<long> matchIds)
        {
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotNullOrEmpty(matchIds, nameof(matchIds));

            var table = await this.GetTableAsync();

            var batch = new TableBatchOperation();
            foreach (var matchId in matchIds)
            {
                batch.Add(TableOperation.Retrieve<PlayedMatchTableEntity>(region.ToString(), matchId.ToString()));
            }

            var result = await table.ExecuteBatchAsync(batch);

            return result
                .Select(f => f.Result)
                .Cast<PlayedMatchTableEntity>()
                .Select(f => f.PlayedMatchAsJson)
                .Select(JsonConvert.DeserializeObject<PlayedMatch>)
                .ToList();
        }

        #region Private Methods
        private async Task<CloudTable> GetTableAsync()
        {
            if (this._table != null)
                return this._table;

            this._table = this._client.GetTableReference(Config.Azure.MatchTableName);
            await this._table.CreateIfNotExistsAsync();

            return this._table;
        }
        #endregion

        #region Internal
        private class PlayedMatchTableEntity : TableEntity
        {
            public PlayedMatchTableEntity(string partitionKey, string rowKey)
                : base(partitionKey, rowKey)
            {
            }

            public PlayedMatchTableEntity()
            {
            }

            public string PlayedMatchAsJson { get; set; }
        }
        #endregion
    }
}