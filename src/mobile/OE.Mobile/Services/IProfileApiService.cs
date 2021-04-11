using OE.Models;
using OE.Models.ActivityStreams;
using Refit;
using System.Threading.Tasks;

namespace OE.Mobile.Services
{
	[Headers("Api-Version: 2021-01-24")]
	public interface IProfileApiService
	{
		[Get("/profile/{userId}")]
		Task<Person> GetProfileAsync(string userId);
	}
}
