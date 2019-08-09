namespace Steeltoe.Tooling.Kubernetes
{
    public class KubernetesDotnetAppDockerfile
    {
        public string App { get; set; }
        public string BuildPath { get; set; }
        
        public string BaseImage { get; set; }
        
    }
}
