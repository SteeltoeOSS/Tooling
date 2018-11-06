// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using System.Linq;

namespace Steeltoe.Tooling.Test
{
    public class ToolingTest : IDisposable
    {
        protected readonly Context Context;

        protected readonly MockShell Shell;

        protected StringWriter Console;

        static ToolingTest()
        {
            Settings.DummiesEnabled = true;
            Settings.ParallelExecutionEnabled = false;
            Settings.WaitDuration = 1;
        }

        public ToolingTest()
        {
            var cfg = new Configuration();
            var path = new[] {"sandboxes", Guid.NewGuid().ToString()}.Aggregate(Path.Combine);
            Directory.CreateDirectory(path);
            cfg.Target = "dummy-target";
            Console = new StringWriter();
            Shell = new MockShell();
            Context = new Context(path, cfg, Console, Shell);
        }

        public void Dispose()
        {
            Directory.Delete(Context.ProjectDirectory, true);
        }

        protected void ClearConsole()
        {
            Console.GetStringBuilder().Clear();
        }
    }
}
