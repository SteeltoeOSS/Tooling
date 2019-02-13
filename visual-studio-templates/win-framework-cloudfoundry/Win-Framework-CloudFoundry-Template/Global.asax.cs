using System;
using System.Web.Http;
using System.Web.Mvc;

namespace $safeprojectname$ {
  public class WebApiApplication : System.Web.HttpApplication {
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);

			// Create applications configuration
			ApplicationConfig.Configure("development");

			// Create logging system using configuration
			LoggingConfig.Configure(ApplicationConfig.Configuration);

			// Add management endpoints to application
			ManagementConfig.ConfigureManagementActuators(
					ApplicationConfig.Configuration,
					LoggingConfig.LoggerProvider,
					GlobalConfiguration.Configuration.Services.GetApiExplorer(),
					LoggingConfig.LoggerFactory);

			// Start the management endpoints
			ManagementConfig.Start();
		}
		protected void Application_Stop()
		{
			ManagementConfig.Stop();
	}
	}
}
