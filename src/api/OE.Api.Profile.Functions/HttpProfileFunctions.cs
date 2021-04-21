using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using OE.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OE.Api.Profile
{
	public class HttpProfileFunctions
	{
		[FunctionName(nameof(GetProfileAsync))]
        public async Task<IActionResult> GetProfileAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "profile/{profileId}")] HttpRequestMessage request,
			string profileId, 
            ILogger log)
        {
			if (!request.Authenticate(out var userId))
			{
				return new UnauthorizedResult();
			}

			if (profileId.Equals("me", StringComparison.OrdinalIgnoreCase) || profileId.Equals(userId, StringComparison.OrdinalIgnoreCase))
			{
				var model = new Models.ActivityStreams.Person
				{
					Id = userId,
					Name = new Dictionary<string, string> { { "mul", request.Headers.GetValues(Constants.Headers.UserDisplayName).First() } },
					Username = request.Headers.GetValues(Constants.Headers.UserPrincipalName).First()
				};

				return new OkObjectResult(model);
			}
			else 
			{
				return new NotFoundResult();
			}
        }
    }
}
