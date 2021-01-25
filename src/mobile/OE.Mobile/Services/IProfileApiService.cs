using OE.Models;
using Refit;
using System.Threading.Tasks;

namespace OE.Mobile.Services
{
	[Headers("Api-Version: 2021-01-24")]
	public interface IProfileApiService
	{
		[Get("/profile/{userId}")]
		Task<Profile> GetProfile(string userId);
	}
}
