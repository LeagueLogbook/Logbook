using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Riot;
using Logbook.Server.Infrastructure.Configuration;
using Logbook.Shared;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Logbook.Server.Infrastructure.Riot
{
    public class AnalyzeSummonerMatchHistoryQueue : IAnalyzeSummonerMatchHistoryQueue
    {
        private readonly CloudQueueClient _queueClient;
        private readonly ConcurrentDictionary<int, CloudQueueMessage> _dequeuedMessages;
        private CloudQueue _queue;

        public AnalyzeSummonerMatchHistoryQueue(CloudQueueClient queueClient)
        {
            Guard.NotNull(queueClient, nameof(queueClient));

            this._queueClient = queueClient;

            this._dequeuedMessages = new ConcurrentDictionary<int, CloudQueueMessage>();
        }

        public async Task EnqueueSummonerAsync(int summonerId)
        {
            Guard.NotZeroOrNegative(summonerId, nameof(summonerId));

            var queue = await this.GetQueueAsync();
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(summonerId));

            await queue.AddMessageAsync(message, null, TimeSpan.FromMinutes(Config.Riot.AnalyzeSummonerMatchHistoryEveryMinutes), null, null);
        }

        public async Task<int?> TryDequeueSummonerAsync()
        {
            var queue = await this.GetQueueAsync();
            var message = await queue.GetMessageAsync();

            if (message == null)
                return null;

            var summonerId = JsonConvert.DeserializeObject<int>(message.AsString);
            this._dequeuedMessages.TryAdd(summonerId, message);

            return summonerId;
        }

        public async Task RemoveAsync(int summonerId)
        {
            Guard.NotZeroOrNegative(summonerId, nameof(summonerId));

            CloudQueueMessage message;
            if (this._dequeuedMessages.TryRemove(summonerId, out message))
            {
                var queue = await this.GetQueueAsync();
                await queue.DeleteMessageAsync(message);
            }
        }

        public async Task TryAgainLaterAsync(int summonerId)
        {
            Guard.NotZeroOrNegative(summonerId, nameof(summonerId));

            CloudQueueMessage message;
            if (this._dequeuedMessages.TryRemove(summonerId, out message))
            {
                var queue = await this.GetQueueAsync();
                await queue.UpdateMessageAsync(message, TimeSpan.FromMinutes(Config.Riot.OnErrorTryToAnalyzeSummonerMatchHistoryAgainAfterMinutes), MessageUpdateFields.Visibility);
            }
        }

        private async Task<CloudQueue> GetQueueAsync()
        {
            if (this._queue != null)
                return this._queue;

            this._queue = this._queueClient.GetQueueReference(Config.Azure.AnalyzeSummonerMatchHistoryQueueName);
            await this._queue.CreateIfNotExistsAsync();

            return this._queue;
        }
    }
}