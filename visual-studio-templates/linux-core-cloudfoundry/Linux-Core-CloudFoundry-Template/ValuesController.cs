using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace $safeprojectname$.Controllers
{
	[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
	private CloudFoundryApplicationOptions _appOptions;
	private CloudFoundryServicesOptions _serviceOptions;
	private readonly ILogger _logger;

	public ValuesController(IOptions<CloudFoundryApplicationOptions> appOptions,
										IOptions<CloudFoundryServicesOptions> serviceOptions,
										ILogger<ValuesController> logger)
	{
		_appOptions = appOptions.Value;
		_serviceOptions = serviceOptions.Value;
		_logger = logger;

		_logger.LogInformation("Hi There");
		/*
		_logger.LogCritical("Test Critical message");
		_logger.LogError("Test Error message");
		_logger.LogWarning("Test Warning message");
		_logger.LogInformation("Test Informational message");
		_logger.LogDebug("Test Debug message");
		_logger.LogTrace("Test Trace message");
		*/
	}

	// GET api/values
	[HttpGet]
	public ActionResult<IEnumerable<string>> Get()
	{
		string appName = _appOptions.ApplicationName;
		string appInstance = _appOptions.ApplicationId;
		/*_serviceOptions.Services["user-provided"]
								.First(q => q.Name.Equals("xxxxxxx"))
								.Credentials["xxxxxxx"].Value*/

		return new string[] { appInstance.ToString(), appName };
	}
	// GET api/values/5
	[HttpGet("{id}")]
	public ActionResult<string> Get(int id)
	{
		return "value";
	}

	// POST api/values
	[HttpPost]
	public void Post([FromBody] string value)
	{
	}

	// PUT api/values/5
	[HttpPut("{id}")]
	public void Put(int id, [FromBody] string value)
	{
	}

	// DELETE api/values/5
	[HttpDelete("{id}")]
	public void Delete(int id)
	{
	}
}
}
