using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using OE.Api.Data.Entities;

namespace OE.Api.Outbox.Functions.Extensions
{
	public static class DocumentExtensions
	{
		public static OutboxEntity ToOutboxEntity(this Document document)
		{
			var json = document.ToString();
			return JsonConvert.DeserializeObject<OutboxEntity>(json);
		}
	}
}
