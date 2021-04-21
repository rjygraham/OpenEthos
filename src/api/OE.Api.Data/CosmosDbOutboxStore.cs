using Microsoft.Azure.Cosmos;
using OE.Api.Data.Entities;
using System.Net;
using System.Threading.Tasks;

namespace OE.Api.Data
{
	public class CosmosDbOutboxStore : IOutboxStore
	{
		private readonly Container outbox;

		public CosmosDbOutboxStore(CosmosClient cosmosClient)
		{
			outbox = cosmosClient.GetContainer(Constants.CosmosDb.DatabaseId, Constants.CosmosDb.OutboxContainerId);
		}

		public async Task<bool> SaveOutboxEntityAsync(OutboxEntity item)
		{
			var partitionKey = new PartitionKey(item.UserId);
			var result = await outbox.CreateItemAsync(item, partitionKey);
			return result.StatusCode == HttpStatusCode.Created;
		}
	}
}
