﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace $safeprojectname$
{
	public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		// Add managment endpoint services
		services.AddCloudFoundryActuators(Configuration);

		services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

		// Setup Options framework with DI
		services.AddOptions();

		// Add Steeltoe Cloud Foundry Options to service container
		services.ConfigureCloudFoundryOptions(Configuration);
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseHsts();
		}

		// Add management endpoints into pipeline
		app.UseCloudFoundryActuators();

		app.UseMvc();
	}
}
}