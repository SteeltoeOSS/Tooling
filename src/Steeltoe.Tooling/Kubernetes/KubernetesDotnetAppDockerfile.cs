namespace Steeltoe.Tooling.Kubernetes
{
    /// <summary>
    /// Represents a Dockerfile for a Dotnet application.
    /// </summary>
    public class KubernetesDotnetAppDockerfile
    {
        /// <summary>
        /// Dotnet application name.
        /// </summary>
        public string App { get; set; }
        
        /// <summary>
        /// Path to Dotnet application build.
        /// </summary>
        public string BuildPath { get; set; }
        
        /// <summary>
        /// Base Docker image form which to build Dotnet application image.
        /// </summary>
        public string BaseImage { get; set; }
    }
}
