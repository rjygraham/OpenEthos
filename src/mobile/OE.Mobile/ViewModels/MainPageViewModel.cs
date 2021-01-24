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

		public MainPageViewModel(INavigationService navigationService, IProfileApiService profileApiService)
			: base(navigationService)
		{
			this.profileApiService = profileApiService;
			InvokeApiCommand = new AsyncCommand(ExecuteInvokeApiCommand);

			Username = $"Hi! It's currently {DateTime.Now}";
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
