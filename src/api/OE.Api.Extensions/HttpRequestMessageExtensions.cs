using Newtonsoft.Json;
using OE.Models.ActivityStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OE.Api.Extensions
{
	public static class HttpRequestMessageExtensions
	{
		public static bool Authenticate(this HttpRequestMessage request, out string userId)
		{
			var result = request.Headers.TryGetValues(Constants.Headers.UserObjectId, out IEnumerable<string> values);

			userId = result
				? values.First()
				: null;

			return result;
		}

		public static async Task<T> ToModelAsync<T>(this HttpRequestMessage request)
			where T: class
		{
			var json = await request.Content.ReadAsStringAsync();
			if (string.IsNullOrWhiteSpace(json))
			{
				return null;
			}

			return JsonConvert.DeserializeObject<T>(json);
		}

		public static async Task<T> ToActivityStreamsModelAsync<T>(this HttpRequestMessage request)
			where T: ActivityStreamsObject
		{
			var json = await request.Content.ReadAsStringAsync();
			if (string.IsNullOrWhiteSpace(json))
			{
				return null;
			}

			var model = JsonConvert.DeserializeObject<T>(json);
			if (model == null)
			{
				return null;
			}

			model.Id = Guid.NewGuid().ToString();
			model.Published = DateTimeOffset.UtcNow;

			return model;
		}

		public static async Task<Activity> ToActivityStreamsActivityModelAsync(this HttpRequestMessage request, string userId)
		{
			var model = await request.ToActivityStreamsModelAsync<Activity>();
			if (model == null)
			{
				return null;
			}

			var displayName = request.Headers.GetValues(Constants.Headers.UserDisplayName).First();
			model.Actor = new HashSet<ActivityStreamsBase>
			{
				new Person
				{
					Id = userId,
					Name = new Dictionary<string, string> { { "mul", displayName } }
				}
			};

			return model;
		}
	}
}
