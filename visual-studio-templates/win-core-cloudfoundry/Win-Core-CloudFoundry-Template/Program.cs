using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;

namespace $safeprojectname$
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseCloudFoundryHosting()
				.AddCloudFoundry()
				.UseStartup<Startup>()
				.ConfigureLogging((builderContext, loggingBuilder) => {
					// Add Steeltoe Dynamic Logging provider
					loggingBuilder.AddDynamicConsole();
				});
	}
}