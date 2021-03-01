using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OE.Api.Extensions;
using OE.Api.Profile;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(Startup))]

namespace OE.Api.Profile
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			JsonConvert.DefaultSettings = () => Constants.Serialization.JsonSerializerSettings;
		}
	}
}
