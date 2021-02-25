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
			var success = await authenticationService.SignUpAsync("eyJhbGciOiJSUzI1NiIsImtpZCI6IjBEMjA0RTMyQTZEQkM5MUJFNDEwMTJBMjc1NjA1MUMxNDA1M0E0QUIiLCJ4NXQiOiJEU0JPTXFiYnlSdmtFQktpZFdCUndVQlRwS3MiLCJ0eXAiOiJKV1QifQ.eyJlbWFpbCI6InJqeWdyYWhhbUBsaXZlLmNvbSIsInNwb25zb3IiOiIxZTM4MWFjYi05ZTIyLTRmYjMtOGE5OC1jMjQ0ZTU2MDliNWUiLCJuYmYiOjE2MTQxNDQwMDcsImV4cCI6MTYxNDMxNjgwNywiaXNzIjoiaHR0cHM6Ly90d2l0Y2hvZWRldi5iMmNsb2dpbi5jb20vYjYyM2I1ZGItYzE2MC00MTBjLWFiOTgtNWM4YmI0ZDBhMDZjL3YyLjAvIiwiYXVkIjoiY2Q0MzFmZjktZmQ2OC00NjZhLWFmNzQtZjA3MDliMjhmMTNhIn0.r1RPDebL2uS2bAIWKErjtcku1vd_EFS3afGbF8i7-H3J2wekkkODwWfM3Z9xAgWQU-QBO3rhzkYsgLNRrFpN2D3qPZfRZV_F5e9dQSVvUujsfXsjlrw8ihDCK_i1jkRR-OfPe-y1nbuTV9HHCruooLqjiGenCr5Ff0zGU9GeNJbupRzrjVVerWbAx1jiGJXjUi_Kwr-pwe2OODX6CzIde5z16vCY4OfL7qtCB63PYkEDYpHdhZpJfKZvfNtHuab0K5FRVQmnouMSlJvauL0YWMUHayWRh_q99IhxyWXnuAaUEhjZEnagbekvh4Pcq3XBsrqmVsYtW0P1gJvXCu0FEQ", "live.com");
			//var success = await authenticationService.LoginAsync();
			if (success)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					IsAuthenticated = !string.IsNullOrEmpty(authenticationService.AccessToken);
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
					IsAuthenticated = !string.IsNullOrEmpty(authenticationService.AccessToken);
				});
			}
		}

		public AsyncCommand InvokeApiCommand { get; private set; }
		private async Task ExecuteInvokeApiCommand()
		{
			var profileResponse = await profileApiService.GetProfile("69b918ec-69bb-489c-be71-2900e6a8cbe7").ConfigureAwait(false);

			Device.BeginInvokeOnMainThread(() =>
			{
				Username = profileResponse.Username;
				DisplayName = profileResponse.DisplayName;
			});
		}
	}
}
