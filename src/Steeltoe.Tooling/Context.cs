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

using System.IO;

namespace Steeltoe.Tooling
{
    public class Context
    {
        public string ProjectDirectory { get; }

        public Configuration Configuration { get; }

        public TextWriter Console { get; }

        public Shell Shell { get; }

        public Target Target
        {
            get
            {
                if (Configuration?.Target == null)
                {
                    throw new ToolingException("Target not set");
                }

                return Registry.GetTarget(Configuration.Target);
            }
        }

        public IBackend Backend => Target.GetBackend(this);

        public Context(string dir, Configuration config, TextWriter console, Shell shell)
        {
            ProjectDirectory = dir;
            Configuration = config;
            Console = console;
            Shell = shell;
        }
    }
}
