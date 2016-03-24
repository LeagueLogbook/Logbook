using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Riot;
using Logbook.Server.Infrastructure.Configuration;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.MatchHistory;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace Logbook.Server.Infrastructure.Riot
{
    public class MatchStorage : IMatchStorage
    {
        private readonly CloudBlobClient _client;
        private CloudBlobContainer _container;

        public MatchStorage(CloudBlobClient client)
        {
            Guard.NotNull(client, nameof(client));

            this._client = client;
        }

        public async Task SaveMatchAsync(PlayedMatch match)
        {
            Guard.NotNull(match, nameof(match));
            
            var container = await this.GetContainerAsync();

            var blobId = this.GenerateBlobId(match.Region, match.MatchId);
            var blob = container.GetBlockBlobReference(blobId);

            if (await blob.ExistsAsync())
                return;

            string json = JsonConvert.SerializeObject(match);
            await blob.UploadTextAsync(json);
        }

        public async Task<IList<PlayedMatch>> GetMatchesAsync(Region region, IList<long> matchIds)
        {
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotNullOrEmpty(matchIds, nameof(matchIds));

            var container = await this.GetContainerAsync();
            
            var tasks = matchIds
                .Select(f => this.GenerateBlobId(region, f))
                .Select(f => container.GetBlockBlobReference(f))
                .Select(f => f.DownloadTextAsync())
                .ToList();

            await Task.WhenAll(tasks);

            return tasks
                .Select(f => f.Result)
                .Select(JsonConvert.DeserializeObject<PlayedMatch>)
                .ToList();
        }

        #region Private Methods
        private string GenerateBlobId(Region region, long matchId)
        {
            return $"{region}/{matchId}";
        }
        private async Task<CloudBlobContainer> GetContainerAsync()
        {
            if (this._container != null)
                return this._container;

            this._container = this._client.GetContainerReference(Config.Azure.MatchContainerName);
            await this._container.CreateIfNotExistsAsync();
            
            return this._container;
        }
        #endregion
    }
}