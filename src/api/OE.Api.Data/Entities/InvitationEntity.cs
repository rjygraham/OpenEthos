﻿using Newtonsoft.Json;
using System;

namespace OE.Api.Data.Entities
{
	public class InvitationEntity
	{
		[JsonProperty("id")]
		public string EmailAddress { get; set; }

		[JsonProperty("inviteeName")]
		public string InviteeName { get; set; }

		[JsonProperty("ancestorId")]
		public string AncestorId { get; set; }

		[JsonProperty("inviterName")]
		public string InviterName { get; set; }

		[JsonProperty("createTimestamp")]
		public DateTimeOffset CreateTimestamp { get; set; }

		[JsonProperty("expirationTimestamp")]
		public DateTimeOffset ExpirationTimestamp { get; set; }

		[JsonProperty("redeemedTimestamp")]
		public DateTimeOffset? RedeemedTimestamp { get; set; }
	}
}
