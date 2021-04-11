using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using OE.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OE.Api.Identity.Functions.Extensions
{
	public static class DocumentExtensions
	{
		public static InvitationEntity ToInvitationEntity(this Document document)
		{
			var json = document.ToString();
			return JsonConvert.DeserializeObject<InvitationEntity>(json);
		}
	}
}
