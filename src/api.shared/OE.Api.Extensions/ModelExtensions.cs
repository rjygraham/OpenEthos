using OE.Api.Data.Entities;
using OE.Models.ActivityStreams;

namespace OE.Api.Extensions
{
	public static class ModelExtensions
	{
		public static OutboxEntity ToOutboxEntity(this Activity model, string userId)
		{
			return new OutboxEntity
			{
				Id = model.Id,
				UserId = userId,
				Timestamp = model.Published.Value.ToUnixTimeMilliseconds(),
				Data = model
			};
		}
	}
}
