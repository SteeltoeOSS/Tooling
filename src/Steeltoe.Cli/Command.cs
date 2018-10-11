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
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Steeltoe.Tooling;
using Steeltoe.Tooling.Executor;

namespace Steeltoe.Cli
{
    public abstract class Command
    {
        private static readonly ILogger Logger = Logging.LoggerFactory.CreateLogger<Command>();

        private readonly IConsole _console;

        protected Command(IConsole console)
        {
            _console = console;
        }

        protected int OnExecute(CommandLineApplication app)
        {
            try
            {
                var executor = GetExecutor();
                var minfo = executor.GetType();
                Logger.LogDebug($"executor is {minfo}");
                var requiresInitializedProject = false;
                foreach (var attr in minfo.GetCustomAttributes(true))
                {
                    var initAttr = attr as RequiresInitializationAttribute;
                    if (initAttr != null)
                    {
                        Logger.LogDebug($"{minfo} requires initialized project");
                        requiresInitializedProject = true;
                    }
                }

                Logger.LogDebug($"tooling working directory is {app.WorkingDirectory}");
                var cfgFilePath = Program.ProjectConfigurationPath;
                if (cfgFilePath == null)
                {
                    cfgFilePath = app.WorkingDirectory;
                }

                var cfgFile = new ToolingConfigurationFile(cfgFilePath);
                Logger.LogDebug($"tooling configuration file is {cfgFile.File}");
                if (requiresInitializedProject && !cfgFile.Exists())
                {
                    throw new ToolingException("Project has not been initialized for Steeltoe Developer Tools");
                }


                var context = new Context(
                    app.WorkingDirectory,
                    cfgFile.ToolingConfiguration,
                    _console.Out,
                    new CommandShell()
                );
                GetExecutor().Execute(context);
                return 0;
            }
            catch (ArgumentException e)
            {
                if (!string.IsNullOrEmpty(e.Message))
                {
                    app.Error.WriteLine(e.Message);
                }

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
                Logger.LogDebug($"unhandled exception: {e}{System.Environment.NewLine}{e.StackTrace}");
                app.Error.WriteLine(e.Message);
                return -1;
            }
        }

        protected abstract IExecutor GetExecutor();
    }
}
