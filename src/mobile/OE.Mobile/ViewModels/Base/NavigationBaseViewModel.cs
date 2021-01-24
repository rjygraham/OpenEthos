using Prism.Navigation;
using Xamarin.CommunityToolkit.ObjectModel;

namespace OE.Mobile.ViewModels
{
	public abstract class NavigationBaseViewModel: ObservableObject
	{
		protected INavigationService NavigationService { get; private set; }

		protected NavigationBaseViewModel(INavigationService navigationService)
		{
			NavigationService = navigationService;
		}
	}
}
