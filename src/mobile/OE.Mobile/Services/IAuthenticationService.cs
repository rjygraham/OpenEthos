using System.Threading.Tasks;

namespace OE.Mobile.Services
{
	public interface IAuthenticationService
	{
		Task<bool> SignUpAsync(string idTokenHint, string domainHint);
		Task<bool> LoginAsync();
		Task<bool> LogoutAsync();
		Task<string> GetAccessTokenAsync();
	}
}
