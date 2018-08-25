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
using McMaster.Extensions.CommandLineUtils;
using Steeltoe.Tooling.Executor;

namespace Steeltoe.Tooling.Cli
{
    public abstract class Command
    {
        protected virtual int OnExecute(CommandLineApplication app)
        {
            var configFile = Path.Combine(Directory.GetCurrentDirectory(), Configuration.DefaultFileName);
            var config = File.Exists(configFile) ? Configuration.Load(configFile) : new Configuration();
            try
            {
                //
                // TODO: Is there a better way to signal that config needs to be stores?  Perhaps a dirty bit?
                //
                if (GetExecutor().Execute(config, new CommandShell(), app.Out))
                {
                    config.Store(configFile);
                }

                return 0;
            }
            catch (ArgumentException e)
            {
                app.Error.WriteLine(e.Message);
                return 1;
            }
            catch (ToolingException e)
            {
                if (!string.IsNullOrEmpty(e.Message))
                {
                    app.Error.WriteLine(e.Message);
                }

                return 2;
            }
            catch (Exception e)
            {
                app.Error.WriteLine(e);
                return -1;
            }
        }

        protected abstract IExecutor GetExecutor();
    }
}
