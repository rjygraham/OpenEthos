using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using OE.Api.Data;
using OE.Api.Data.Entities;
using OE.Api.Outbox.Functions.Extensions;
using OE.Models.ActivityStreams;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OE.Api.Outbox.Functions
{
	public class CosmosDbOutboxFunctions
    {
		private readonly IInvitationStore invitationStore;

		public CosmosDbOutboxFunctions(IInvitationStore invitationStore)
		{
			this.invitationStore = invitationStore;
		}

        [FunctionName(nameof(ProcessOutboxItemAsync))]
        public async Task ProcessOutboxItemAsync(
			[CosmosDBTrigger(Constants.CosmosDb.DatabaseId, Constants.CosmosDb.OutboxContainerId, ConnectionStringSetting = "CosmosDbSqlConnection", LeaseCollectionName = Constants.CosmosDb.LeasesContainerId, CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<Microsoft.Azure.Documents.Document> documents,
			ILogger log
		)
        {
			if (documents != null)
			{
				foreach (var document in documents)
				{
					var entity = document.ToOutboxEntity();

					switch (entity.Data.Type)
					{
						case nameof(Invite):
							await ProcessInviteAsync(entity);
							break;
						default:
							break;
					}
				}
			}
		}

		private async Task ProcessInviteAsync(OutboxEntity outboxEntity)
		{
			var invite = (Invite)outboxEntity.Data;
			var to = (Person)invite.To.First();

			var invitationEntity = new InvitationEntity
			{
				EmailAddress = to.Username,
				AncestorId = outboxEntity.UserId,
				InviteeName = to.Name["mul"],
				CreateTimestamp = invite.Published.Value,
				ExpirationTimestamp = invite.Published.Value.AddHours(48)
			};

			var success = await invitationStore.CreateInvitationAsync(invitationEntity);
		}
	}
}
