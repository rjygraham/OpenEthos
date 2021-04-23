using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Abstractions;
using Yarp.ReverseProxy.Service;
using System.Collections.Generic;
using System.Threading;

namespace OE.DevProxy.Config
{
    public static class TyeConfigProviderExtensions
	{
		public static IReverseProxyBuilder LoadFromMemory(this IReverseProxyBuilder builder, IReadOnlyList<ProxyRoute> routes, IReadOnlyList<Cluster> clusters)
		{
			builder.Services.AddSingleton<IProxyConfigProvider>(new InMemoryConfigProvider(routes, clusters));
			return builder;
		}
	}

	public class InMemoryConfigProvider : IProxyConfigProvider
	{
        private InMemoryConfig config;

        public InMemoryConfigProvider(IReadOnlyList<ProxyRoute> routes, IReadOnlyList<Cluster> clusters)
		{
            config = new InMemoryConfig(routes, clusters);
		}

		public IProxyConfig GetConfig()
		{
            return config;
		}

        private class InMemoryConfig : IProxyConfig
        {
            // Used to implement the change token for the state
            private readonly CancellationTokenSource cts = new CancellationTokenSource();

            public InMemoryConfig(IReadOnlyList<ProxyRoute> routes, IReadOnlyList<Cluster> clusters)
            {
                Routes = routes;
                Clusters = clusters;
                ChangeToken = new CancellationChangeToken(cts.Token);
            }

            /// <summary>
            /// A snapshot of the list of routes for the proxy
            /// </summary>
            public IReadOnlyList<ProxyRoute> Routes { get; }

            /// <summary>
            /// A snapshot of the list of Clusters which are collections of interchangable destination endpoints
            /// </summary>
            public IReadOnlyList<Cluster> Clusters { get; }

            /// <summary>
            /// Fired to indicate the the proxy state has changed, and that this snapshot is now stale
            /// </summary>
            public IChangeToken ChangeToken { get; }

            internal void SignalChange()
            {
                cts.Cancel();
            }
        }
    }
}
