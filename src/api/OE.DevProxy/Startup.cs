using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ReverseProxy.Abstractions;
using OE.DevProxy.Config;
using OE.DevProxy.Transformations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace OE.DevProxy
{
	public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddReverseProxy()
                .LoadFromMemory(GetRoutes(), GetClusters())
                //.LoadFromConfig(Configuration.GetSection("ReverseProxy"))
                .AddTransforms<JwtHeaderTransformation>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Override all validation for local development.
					options.TokenValidationParameters = new TokenValidationParameters
					{
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = false,
                        ValidateLifetime = true,
                        RequireExpirationTime = false,
                        RequireSignedTokens = false,
                        SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                        {
                            var jwt = new JwtSecurityToken(token);
                            return jwt;
                        },
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("fakeTokenPolicy", policy =>
                    policy.RequireAuthenticatedUser());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapReverseProxy();
            });
        }

        private Cluster[] GetClusters()
        {
			var identity = Configuration.GetServiceUri("identity");
			var profile = Configuration.GetServiceUri("profile");
			var outbox = Configuration.GetServiceUri("outbox");
			var inbox = Configuration.GetServiceUri("inbox");

            return new[]
            {
				new Cluster()
				{
					Id = "identityCluster",
					Destinations = new Dictionary<string, Destination>(StringComparer.OrdinalIgnoreCase)
					{
						{ "identityCluster", new Destination() { Address = $"{identity}api" } }
					}
				},
				new Cluster()
				{
					Id = "profileCluster",
					Destinations = new Dictionary<string, Destination>(StringComparer.OrdinalIgnoreCase)
					{
						{ "profileCluster", new Destination() { Address = $"{profile}api" } }
					}
				},
				new Cluster()
                {
                    Id = "outboxCluster",
                    Destinations = new Dictionary<string, Destination>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "outboxCluster", new Destination() { Address = $"{outbox}api" } }
                    }
                },
                new Cluster()
                {
                    Id = "inboxCluster",
                    Destinations = new Dictionary<string, Destination>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "inboxCluster", new Destination() { Address = $"{inbox}api" } }
                    }
                }
            };
        }

        private ProxyRoute[] GetRoutes()
        {
            return new[]
            {
				new ProxyRoute()
				{
					RouteId = "identityRoute",
					ClusterId = "identityCluster",
					Match = new ProxyMatch
					{
						Path = "/identity/{**catch-all}"
					}
				},
				new ProxyRoute()
				{
					RouteId = "profileRoute",
					ClusterId = "profileCluster",
					AuthorizationPolicy = "fakeTokenPolicy",
					Match = new ProxyMatch
					{
						Path = "/profile/{**catch-all}"
					},
					Metadata = new Dictionary<string, string>
					{
						{ "ExtractHeaders", "true" }
					}
				},
				new ProxyRoute()
                {
                    RouteId = "outboxRoute",
                    ClusterId = "outboxCluster",
                    AuthorizationPolicy = "fakeTokenPolicy",
                    Match = new ProxyMatch
                    {
                        Path = "/outbox/{**catch-all}"
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "ExtractHeaders", "true" }
                    }
                },
                new ProxyRoute()
                {
                    RouteId = "inboxRoute",
                    ClusterId = "inboxCluster",
                    AuthorizationPolicy = "fakeTokenPolicy",
                    Match = new ProxyMatch
                    {
                        Path = "/inbox/{**catch-all}"
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "ExtractHeaders", "true" }
                    }
                }
            };
        }
    }
}
