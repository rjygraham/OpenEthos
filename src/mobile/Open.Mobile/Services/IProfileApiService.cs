using Open.Models;
using Refit;
using System.Threading.Tasks;

namespace Open.Mobile.Services
{
	public interface IProfileApiService
	{
		[Get("/api/{userId}")]
		Task<Profile> GetProfile(string userId);
	}
}
