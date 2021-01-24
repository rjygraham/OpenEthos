using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ModelsProfile = OE.Models.Profile;

namespace OE.Api.Profile
{
	public class HttpProfileFunctions
	{
		private string mockId = "69b918ec-69bb-489c-be71-2900e6a8cbe7";

		[FunctionName(nameof(GetProfileAsync))]
        public async Task<IActionResult> GetProfileAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{id}")] HttpRequest req,
			string id, 
            ILogger log)
        {
			if (mockId.Equals(id, StringComparison.OrdinalIgnoreCase))
			{
				return new OkObjectResult(new ModelsProfile { Id = id, Username = "@ryan", DisplayName = "Ryan" });
			}
			else 
			{
				return new NotFoundResult();
			}
			
        }
    }
}
