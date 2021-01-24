using OE.Models;
using Refit;
using System.Threading.Tasks;

namespace OE.Mobile.Services
{
	public interface IProfileApiService
	{
		[Get("/api/{userId}")]
		Task<Profile> GetProfile(string userId);
	}
}
