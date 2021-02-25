using OE.Mobile.Services;
using Prism.Navigation;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace OE.Mobile.ViewModels
{
	public class SignUpPageViewModel : NavigationBaseViewModel, INavigationAware
	{
		private string idTokenHint;
		private readonly IAuthenticationService authenticationService;

		public SignUpPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
			: base(navigationService)
		{
			this.authenticationService = authenticationService;

			SignUpCommand = new AsyncCommand(ExecuteLoginCommand);
		}

		public AsyncCommand SignUpCommand { get; private set; }

		private async Task ExecuteLoginCommand()
		{
			var success = await authenticationService.SignUpAsync(idTokenHint, "live.com");

			if (success)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					var isAuthenticated = !string.IsNullOrEmpty(authenticationService.AccessToken);
				});
			}
		}

		public void OnNavigatedFrom(INavigationParameters parameters)
		{
		}

		public void OnNavigatedTo(INavigationParameters parameters)
		{
			idTokenHint = parameters.GetValue<string>("idTokenHint");
		}
	}
}
