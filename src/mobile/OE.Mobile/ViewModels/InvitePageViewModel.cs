using OE.Mobile.Services;
using OE.Models.ActivityStreams;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace OE.Mobile.ViewModels
{
	public class InvitePageViewModel : NavigationBaseViewModel
	{
		private readonly IApiService apiService;

		private string name;
		public string Name
		{
			get => name;
			set => SetProperty(ref name, value);
		}

		private string emailAddress;
		public string EmailAddress
		{
			get => emailAddress;
			set => SetProperty(ref emailAddress, value);
		}

		public InvitePageViewModel(INavigationService navigationService, IApiService apiService)
			: base(navigationService)
		{
			this.apiService = apiService;

			SendInviteCommand = new AsyncCommand(ExecuteSendInviteCommand, allowsMultipleExecutions: false);
		}

		public IAsyncCommand SendInviteCommand { get; set; }

		private async Task ExecuteSendInviteCommand()
		{
			var invite = new Invite
			{
				To = new HashSet<ActorBase>
				{
				new Person
					{
						Name = new Dictionary<string, string> { { "mul", this.Name } },
						Username = EmailAddress
					}
				}
			};

			await apiService.SaveOutboxItemAsync(invite);
		}
	}
}
