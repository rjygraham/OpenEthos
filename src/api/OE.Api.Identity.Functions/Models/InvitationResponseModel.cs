using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OE.Api.Identity.Functions.Models
{
	public class InvitationResponseModel
	{
		[JsonProperty("ancestor")]
		public string AncestorId { get; set; }

		[JsonProperty("sponsor")]
		public string SponsorId { get; set; }
	}
}
