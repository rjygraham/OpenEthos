using Microsoft.ReverseProxy.Abstractions.Config;
using OE.Api.Extensions;

namespace OE.DevProxy.Transformations
{
	public class JwtHeaderTransformation : ITransformProvider
	{
		public void Apply(TransformBuilderContext context)
		{
			string value = null;
			if (context.Route.Metadata?.TryGetValue("ExtractHeaders", out value) ?? false)
			{
				context.AddRequestTransform(transformContext =>
				{
					transformContext.ProxyRequest.Headers.Add(Constants.Headers.UserObjectId, "3fb8ee60-d151-4e40-9cb6-dc59e287bbbb");
					transformContext.ProxyRequest.Headers.Add(Constants.Headers.UserObjectId, "rjygraham@openethos.io");
					transformContext.ProxyRequest.Headers.Add(Constants.Headers.UserObjectId, "Ryan Graham");
					return default;
				});
			}	
		}

		public void ValidateCluster(TransformClusterValidationContext context)
		{
		}

		public void ValidateRoute(TransformRouteValidationContext context)
		{
		}
	}
}
