using OE.Mobile.Services;
using OE.Mobile.ViewModels;
using OE.Mobile.Views;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Refit;
using System;

namespace OE.Mobile
{
	public partial class App
	{
		public App()
			: this(null)
		{
		}

		public App(IPlatformInitializer initializer)
			: base(initializer)
		{
		}

		protected override async void OnInitialized()
		{
			InitializeComponent();

			await NavigationService.NavigateAsync(nameof(MainPage));
		}

		protected override void OnResume()
		{
			base.OnResume();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
			containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();

			containerRegistry.RegisterSingleton<IAuthenticationService, MsalAuthenticationService>();
			containerRegistry.RegisterInstance<IProfileApiService>(RestService.For<IProfileApiService>("https://twitch-openethos-dev-apim.azure-api.net"));
		}

		protected override void OnAppLinkRequestReceived(Uri uri)
		{
			base.OnAppLinkRequestReceived(uri);

			var idTokenHint = uri.Query.Split('=')[1];
			
			var navParameters = new NavigationParameters
			{
				{ "idTokenHint", idTokenHint }
			};

			NavigationService.NavigateAsync("/SignUpPage", navParameters);
		}
	}
}
