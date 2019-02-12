using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
		}
		protected void Application_Stop()
		{
		}
	}
}
