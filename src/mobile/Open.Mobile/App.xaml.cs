using Open.Mobile.Services;
using Open.Mobile.ViewModels;
using Open.Mobile.Views;
using Prism;
using Prism.Ioc;
using Refit;

namespace Open.Mobile
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

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

			containerRegistry.RegisterInstance<IProfileApiService>(RestService.For<IProfileApiService>("https://66d47af52655.ngrok.io"));
		}
	}
}
