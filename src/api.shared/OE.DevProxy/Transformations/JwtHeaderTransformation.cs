using Yarp.ReverseProxy.Abstractions.Config;
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
					transformContext.ProxyRequest.Headers.Add(Constants.Headers.UserObjectId, "TODO: GET FROM USER-SECRETS");
					transformContext.ProxyRequest.Headers.Add(Constants.Headers.UserPrincipalName, "TODO: GET FROM USER-SECRETS");
					transformContext.ProxyRequest.Headers.Add(Constants.Headers.UserDisplayName, "TODO: GET FROM USER-SECRETS");
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
