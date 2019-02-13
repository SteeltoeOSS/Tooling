using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace $safeprojectname$ {
	public class ApplicationConfig{
		public static IConfigurationRoot Configuration { get; set; }

		public static void Configure(string environment)
		{

			// Set up configuration sources.
			var builder = new ConfigurationBuilder()
					.SetBasePath(GetContentRoot())
					.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
					.AddJsonFile($"appsettings.{environment}.json", optional: true)
					.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public static string GetContentRoot()
		{
			var basePath = (string)AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") ??
				 AppDomain.CurrentDomain.BaseDirectory;
			return Path.GetFullPath(basePath);
		}
	}
}