using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OE.Api.Identity.Functions.Models
{
	public class InvitationRequestModel
	{
		[JsonProperty("email")]
		public string EmailAddress { get; set; }
	}
}
