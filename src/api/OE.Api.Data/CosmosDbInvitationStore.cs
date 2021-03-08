using OE.Api.Data.Entities;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace OE.Api.Data
{
	public class CosmosDbInvitationStore : IInvitationStore
	{
		private readonly Container invitations;

		public CosmosDbInvitationStore(CosmosClient cosmosClient)
		{
			invitations = cosmosClient.GetContainer(Constants.CosmosDb.DatabaseId, Constants.CosmosDb.InvitationsContainerId);
		}

		public async Task<bool> CreateInvitationAsync(InvitationEntity item)
		{
			var partitionKey = new PartitionKey(item.EmailAddress);
			var result = await invitations.CreateItemAsync(item, partitionKey);
			return result.StatusCode == HttpStatusCode.Created;
		}

		public async Task<InvitationEntity> GetInvitationEntityAsync(string id)
		{
			try
			{
				var partitionKey = new PartitionKey(id);
				ItemResponse<InvitationEntity> invitation = await invitations.ReadItemAsync<InvitationEntity>(id, partitionKey);
				return invitation.Resource;
			}
			catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}
			catch (CosmosException ex)
			{
				throw;
			}
		}

		public async Task<bool> UpdateInvitationEntityAsync(InvitationEntity item)
		{
			var existing = await GetInvitationEntityAsync(item.EmailAddress);

			if (existing == null)
			{
				return false;
			}

			existing.RedeemedTimestamp = DateTimeOffset.UtcNow;

			var partitionKey = new PartitionKey(existing.EmailAddress);
			var result = await invitations.ReplaceItemAsync(existing, existing.EmailAddress, partitionKey);
			return result.StatusCode == HttpStatusCode.OK;
		}
	}
}
