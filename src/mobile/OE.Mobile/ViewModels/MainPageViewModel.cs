using OE.Mobile.Services;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace OE.Mobile.ViewModels
{
	public class MainPageViewModel: NavigationBaseViewModel
	{
		private readonly IAuthenticationService authenticationService;
		private readonly IProfileApiService profileApiService;

		private string username;
		public string Username
		{
			get => username;
			set => SetProperty(ref username, value);
		}

		private string displayName;
		public string DisplayName
		{
			get => displayName;
			set => SetProperty(ref displayName, value);
		}

		private bool isAuthenticated;
		public bool IsAuthenticated
		{
			get => isAuthenticated;
			set => SetProperty(ref isAuthenticated, value);
		}

		public MainPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IProfileApiService profileApiService)
			: base(navigationService)
		{
			this.authenticationService = authenticationService;
			this.profileApiService = profileApiService;

			LoginCommand = new AsyncCommand(ExecuteLoginCommand);
			LogoutCommand = new AsyncCommand(ExecuteLogoutCommand);
			InvokeApiCommand = new AsyncCommand(ExecuteInvokeApiCommand);

			Username = $"Hi! It's currently {DateTime.Now}";
		}

		public AsyncCommand LoginCommand { get; private set; }

		private async Task ExecuteLoginCommand()
		{
			var success = await authenticationService.LoginAsync();
			if (success)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					// Do something here.
				});
			}
		}

		public AsyncCommand LogoutCommand { get; private set; }
		private async Task ExecuteLogoutCommand()
		{
			var success = await authenticationService.LogoutAsync();
			if (success)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					// Do something here.
				});
			}
		}

		public AsyncCommand InvokeApiCommand { get; private set; }
		private async Task ExecuteInvokeApiCommand()
		{
			var profileResponse = await profileApiService.GetProfileAsync("69b918ec-69bb-489c-be71-2900e6a8cbe7").ConfigureAwait(false);

			Device.BeginInvokeOnMainThread(() =>
			{
				Username = profileResponse.Username;
				DisplayName = profileResponse.Name["mul"];
			});
		}
	}
}
