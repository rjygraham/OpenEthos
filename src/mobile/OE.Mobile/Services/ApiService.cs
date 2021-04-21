using OE.Mobile.Handlers;
using OE.Models.ActivityStreams;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace OE.Mobile.Services
{
	public class ApiService : IApiService
	{
		private readonly IOutboxApiService outboxApiService;
		private readonly IProfileApiService profileApiService;

		private readonly Dictionary<int, CancellationTokenSource> runningTasks = new Dictionary<int, CancellationTokenSource>();

		private bool isConnected;


		public ApiService(IAuthenticationService authenticationService)
		{
			var settings = new RefitSettings
			{
				ContentSerializer = new NewtonsoftJsonContentSerializer(ActivityStreamsJsonSerializerSettings.Instance)
			};

			var client = new HttpClient(new AuthenticatedHttpClientHandler(authenticationService)) { BaseAddress = new Uri(Constants.ApiSettings.HostUrl) };

			this.outboxApiService = RestService.For<IOutboxApiService>(client, settings);
			this.profileApiService = RestService.For<IProfileApiService>(client, settings);

			isConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
			Connectivity.ConnectivityChanged += OnConnectivityConnectivityChanged;
		}

		public async Task<Person> GetProfileAsync(string userId)
		{
			return await  RemoteRequestWithReturnAsync(profileApiService.GetProfileAsync(userId));
		}

		public async Task SaveOutboxItemAsync(ActivityStreamsObject outboxItem)
		{
			await RemoteRequestAsync(outboxApiService.SaveOutboxItemAsync(outboxItem))
				.ConfigureAwait(false);
		}

		private void OnConnectivityConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
		{
			isConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;

			if (!isConnected)
			{
				// Cancel All Running Task
				var kvp = runningTasks.ToList();
				foreach (var item in kvp)
				{
					item.Value.Cancel();
					runningTasks.Remove(item.Key);
				}
			}
		}

		private async Task RemoteRequestAsync(Task task)
		{
			runningTasks.Add(task.Id, new CancellationTokenSource());

			await Policy
				.Handle<WebException>()
				.Or<ApiException>()
				.Or<TaskCanceledException>()
				.WaitAndRetryAsync
				(
					retryCount: 2,
					sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
				)
				.ExecuteAsync(async () =>
				{
					await task.ConfigureAwait(false);
					runningTasks.Remove(task.Id);
				})
				.ConfigureAwait(false);
		}

		private async Task<TData> RemoteRequestWithReturnAsync<TData>(Task<TData> task)
		{
			runningTasks.Add(task.Id, new CancellationTokenSource());

			return await Policy
				.Handle<WebException>()
				.Or<ApiException>()
				.Or<TaskCanceledException>()
				.WaitAndRetryAsync
				(
					retryCount: 2,
					sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
				)
				.ExecuteAsync(async () =>
				{
					var result = await task.ConfigureAwait(false);
					runningTasks.Remove(task.Id);
					return result;
				})
				.ConfigureAwait(false);
		}
	}
}
