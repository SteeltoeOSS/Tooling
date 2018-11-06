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

namespace Steeltoe.Tooling.Executor
{
    public class InitializationExecutor : Executor
    {
        private readonly string _path;

        private readonly bool _force;

        public InitializationExecutor(string path = null, bool force = false)
        {
            _path = path;
            _force = force;
        }

        protected override void Execute()
        {
            var path = _path == null
                ? Context.ProjectDirectory
                : Path.Combine(Context.ProjectDirectory, _path);

            var cfgFile = new ConfigurationFile(path);
            if (cfgFile.Exists() && !_force)
            {
                throw new ToolingException("Steeltoe Developer Tools already initialized");
            }

            cfgFile.Store();
            Context.Console.WriteLine("Initialized Steeltoe Developer Tools");
        }
    }
}
