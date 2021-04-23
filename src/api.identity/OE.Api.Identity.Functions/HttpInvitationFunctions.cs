using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using OE.Api.Data;
using OE.Api.Extensions;
using OE.Api.Identity.Functions.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OE.Api.Identity
{
	public class HttpInvitationFunctions
    {
		private readonly IInvitationStore invitationStore;

		public HttpInvitationFunctions(IInvitationStore invitationStore)
		{
			this.invitationStore = invitationStore;
		}

        [FunctionName(nameof(GetInvitationByCodeAsync))]
        public async Task<IActionResult> GetInvitationByCodeAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "invitations/get")] HttpRequestMessage request,
            ILogger log)
        {
			var requestModel = await request.ToModelAsync<InvitationRequestModel>();
			
			var invitation = await invitationStore.GetInvitationEntityAsync(requestModel.EmailAddress);

			if (invitation == null || invitation.ExpirationTimestamp <= DateTimeOffset.UtcNow || invitation.RedeemedTimestamp.HasValue)
			{
				return new ConflictResult();
			}

			var responseModel = new InvitationResponseModel
			{
				AncestorId = invitation.AncestorId,
				SponsorId = invitation.AncestorId
			};

			return new OkObjectResult(responseModel);
        }

		[FunctionName(nameof(RedeemInvitationCodeAsync))]
		public async Task<IActionResult> RedeemInvitationCodeAsync(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "invitations/redeem")] HttpRequestMessage request,
			ILogger log)
		{
			var requestModel = await request.ToModelAsync<InvitationRequestModel>();

			var invitation = await invitationStore.GetInvitationEntityAsync(requestModel.EmailAddress);

			if (invitation == null)
			{
				return new ConflictResult();
			}

			invitation.RedeemedTimestamp = DateTimeOffset.UtcNow;

			var result = await invitationStore.UpdateInvitationEntityAsync(invitation);

			if (result)
			{
				return new OkResult();
			}
			else
			{
				return new ConflictResult();
			}
		}
	}
}
