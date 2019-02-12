
using System;
using System.Collections.Generic;
using System.Web.Http;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Microsoft.Extensions.Logging;

namespace $safeprojectname$.Controllers
{
  public class ValuesController : ApiController
	{
		private CloudFoundryApplicationOptions _appOptions;
		private CloudFoundryServicesOptions _serviceOptions;
		private ILogger<ValuesController> _logger;

		public ValuesController()
		{
			_appOptions = ApplicationConfig.CloudFoundryApplication;
			_serviceOptions = ApplicationConfig.CloudFoundryServices;
			_logger = LoggingConfig.LoggerFactory.CreateLogger<ValuesController>();

			_logger.LogInformation("Hi there, this is a {LogLevel} log", LogLevel.Information.ToString());
			/*
			_logger.LogTrace("This is a {LogLevel} log", LogLevel.Trace.ToString());
			_logger.LogDebug("This is a {LogLevel} log", LogLevel.Debug.ToString());
			_logger.LogInformation("This is a {LogLevel} log", LogLevel.Information.ToString());
			_logger.LogWarning("This is a {LogLevel} log", LogLevel.Warning.ToString());
			_logger.LogError("This is a {LogLevel} log", LogLevel.Error.ToString());
			_logger.LogCritical("This is a {LogLevel} log", LogLevel.Critical.ToString());
			*/
		}

		// GET api/values
		[HttpGet]
		public IEnumerable<string> Get()
		{
			string appName = _appOptions.ApplicationName;
			string appInstance = _appOptions.ApplicationId;
			/*_serviceOptions.Services["user-provided"]
                  .First(q => q.Name.Equals("xxxxxxx"))
									.Credentials["xxxxxxx"].Value*/

			return new string[] { appInstance.ToString(), appName };
		}

		// GET api/values/5
		[HttpGet]
		public string Get(int id)
		{
			return "value";
    }

    // POST api/values
    public void Post([FromBody]string value)
    {
    }

    // PUT api/values/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/values/5
    public void Delete(int id)
    {
    }
	}
}
