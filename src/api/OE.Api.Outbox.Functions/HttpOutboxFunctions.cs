using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using OE.Api.Data;
using OE.Api.Extensions;
using System.Net.Http;
using System.Threading.Tasks;

namespace OE.Api.Outbox
{
	public class HttpOutboxFunctions
    {
		private readonly IOutboxStore outboxStore;

		public HttpOutboxFunctions(IOutboxStore outboxStore)
		{
			this.outboxStore = outboxStore;
		}

        [FunctionName(nameof(CreateOutboxItemAsync))]
        public async Task<IActionResult> CreateOutboxItemAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "outbox")] HttpRequestMessage request,
            ILogger log)
        {
			if (!request.Authenticate(out var userId))
			{
				return new UnauthorizedResult();
			}

			var model = await request.ToActivityStreamsActivityModelAsync(userId);
			if (model == null)
			{
				return new BadRequestResult();
			}

			var entity = model.ToOutboxEntity(userId);

			var success = await outboxStore.SaveOutboxEntityAsync(entity);
			
			return success
				? new StatusCodeResult(StatusCodes.Status201Created)
				: new BadRequestResult();
        }
    }
}
