# Visual Studio Templates

Steeltoe is an open source project that enables .NET developers to implement industry standard best practices when building resilient microservices for the cloud. The Steeltoe client libraries enable .NET Core and .NET Framework apps to easily leverage Netflix Eureka, Hystrix, Spring Cloud Config Server, and Cloud Foundry services.

### Find project templates online

Each project in this solution is published in the Visual Studio Marketplace, for use as a template when you open Visual Studio and choose `File > New > Project > Online`. You can search for `steeltoe` in the new project window to see options.

### Load project templates offline

Alternatly you can load the project templates into Visual Studio manually by downloading the VSIX file in the [Releases](https://github.com/steeltoeoss/tooling/releases) area. Once downloaded, close Visual Studio, and click the VSIX file to begin install. All templates are included in this offline VSIX version. To get stared, create a new project `File > New > Project > Installed > Visual C#`.

## Getting Started

The templates are based on the C# web api template but have been simplified to only offer RESTful endpoints and use Steeltoe for many of the cloud native functions like managing environment variables and streaming logs. Each template targets a specific runtime and operating system combination (ie: .NET Core on Linux).

As a starting point, let Visual Studio load the project completely and simply publish. This will create your build artifact which can then be pushed to the desired platform or cloud.

**NOTE: for the framework projects, you will need to restore all packages. Once Visual Studio has completely loaded the project, right click on `References` and choose `Manage Nuget Packages`. This new window should offer an option at the top to `Restore`. Click it and let Visual Studio retrieve the needed packages to publish the project.

### Base Templates

The templates with `base` naming are platform neutral ie: they can be used anywhere a website can be hosted (iis, public cloud, etc).

### Cloud Foundry Templates

The templates with `cloudfoundry` naming extend the base templates to target deploying the app to [Cloud Foundry](https://cloudfoundry.io). These templates offer additional features like management integrations enhanced log streaming, and each template has a ready to `cf push` manifest.

### Windows Diego Cells on Cloud Foundry

When using the Cloud Foundry templates that target .NET Framework, you will need to ensure your platform has the proper cell deployed. Read more about Windows Diego Cells [here](https://docs.pivotal.io/pivotalcf/windows/index.html).

## More Info

To learn more about Steeltoe, go to the site (https://steeltoe.io)[https://steeltoe.io].