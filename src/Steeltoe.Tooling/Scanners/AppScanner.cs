using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steeltoe.Tooling.Models;

namespace Steeltoe.Tooling.Scanners
{
    /// <summary>
    /// A scanner for applications.
    /// </summary>
    public class AppScanner
    {
        /// <summary>
        /// Scans for applications in the specified path.
        /// </summary>
        /// <param name="path">Path at which to scan.</param>
        /// <returns></returns>
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
