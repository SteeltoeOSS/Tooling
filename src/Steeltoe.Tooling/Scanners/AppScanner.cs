using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Steeltoe.Tooling.Scanners
{
    public class AppScanner
    {
        public List<AppInfo> Scan(string path)
        {
            List<AppInfo> apps = new List<AppInfo>();
            foreach (var project in Directory.GetFiles(path).Where(f => f.EndsWith(".csproj")))
            {
                var app = new AppInfo(Path.GetFileNameWithoutExtension(project));
                apps.Add(app);
            }

            return apps;
        }
    }
}
