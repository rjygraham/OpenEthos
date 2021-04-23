using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace OE.Api.Inbox.Functions
{
	public class HttpInboxFunctions
    {
        [FunctionName(nameof(GetInboxItemsAsync))]
        public async Task<IActionResult> GetInboxItemsAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "inbox")] HttpRequestMessage request,
            ILogger log)
        {
            return new OkResult();
        }
    }
}

