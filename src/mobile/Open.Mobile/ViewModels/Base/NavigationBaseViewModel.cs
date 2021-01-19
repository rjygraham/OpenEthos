using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Open.Mobile.ViewModels.Base
{
	public class NavigationBaseViewModel: ObservableObject
	{
		protected INavigationService NavigationService { get; private set; }

		public NavigationBaseViewModel(INavigationService navigationService)
		{
			this.NavigationService = navigationService;
		}
	}
}
