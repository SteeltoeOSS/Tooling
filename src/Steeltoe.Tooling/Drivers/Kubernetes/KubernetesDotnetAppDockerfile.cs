using System.Collections.Generic;

namespace Steeltoe.Tooling.Drivers.Kubernetes
{
    /// <summary>
    /// Represents a Dockerfile for a Dotnet application.
    /// </summary>
    public class KubernetesDotnetAppDockerfile
    {
        /// <summary>
        /// Base Docker image form which to build Dotnet application image.
        /// </summary>
        public string BaseImage { get; set; }

        /// <summary>
        /// Dotnet application name.
        /// </summary>
        public string App { get; set; }

        /// <summary>
        /// Path containing Dotnet application.
        /// </summary>
        public string AppPath { get; set; }

        /// <summary>
        /// Path to Dotnet application build.
        /// </summary>
        public string BuildPath { get; set; }

        /// <summary>
        /// Docker container environment.
        /// </summary>
        public string Environment { get; set; }
    }
}
