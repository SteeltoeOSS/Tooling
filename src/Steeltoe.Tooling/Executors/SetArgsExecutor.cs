// Copyright 2018 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Steeltoe.Tooling.Executors
{
    /// <summary>
    /// A workflow to set the arguments of an application or service.
    /// </summary>
    [RequiresInitialization]
    public class SetArgsExecutor : AppOrServiceExecutor
    {
        private readonly string _target;

        private readonly string _args;

        private readonly bool _force;

        /// <summary>
        /// A workflow to set the arguments for an application or service.
        /// </summary>
        /// <param name="appOrServiceName">Application or service name.</param>
        /// <param name="args">Application or service arguments.</param>
        /// <param name="force">Force replacement of existing arguments.</param>
        public SetArgsExecutor(string appOrServiceName, string args, bool force = false)
            : this(appOrServiceName, null, args, force)
        {
        }

        /// <summary>
        /// A workflow to set the arguments for an application or service.
        /// If <code>target</code> is null, set the application or service arguments.
        /// If <code>target</code> is not null, set the arguments for deploying the application or service.
        /// </summary>
        /// <param name="appOrServiceName">Application or service name.</param>
        /// <param name="target">Deployment target name (can be null).</param>
        /// <param name="args">Application or service arguments.</param>
        /// <param name="force">Force replacement of existing arguments.</param>
        public SetArgsExecutor(string appOrServiceName, string target, string args, bool force = false)
            : base(appOrServiceName)
        {
            _target = target;
            _args = args;
            _force = force;
        }

        /// <summary>
        /// Set the application arguments.
        /// </summary>
        internal override void ExecuteForApp()
        {
            if (_target == null)
            {
                ExecuteSetAppArgs();
            }
            else
            {
                ExecuteSetAppTargetArgs();
            }
        }

        /// <summary>
        /// Set the service arguments.
        /// </summary>
        internal override void ExecuteForService()
        {
            if (_target == null)
            {
                ExecuteSetServiceArgs();
            }
            else
            {
                ExecuteSetServiceTargetArgs();
            }
        }

        private void ExecuteSetAppArgs()
        {
            var appName = AppOrServiceName;
            var args = Context.Configuration.GetAppArgs(appName);
            if (args != null && !_force)
            {
                throw new ToolingException($"'{appName}' app args already set to '{args}'");
            }

            Context.Configuration.SetAppArgs(appName, _args);
            Context.Console.WriteLine($"Set '{appName}' app args to '{_args}'");
        }

        private void ExecuteSetAppTargetArgs()
        {
            var appName = AppOrServiceName;
            var args = Context.Configuration.GetAppArgs(appName, _target);
            if (args != null && !_force)
            {
                throw new ToolingException($"'{_target}' deploy args for '{appName}' app already set to '{args}'");
            }

            Context.Configuration.SetAppArgs(appName, _target, _args);
            Context.Console.WriteLine($"Set '{_target}' deploy args for '{appName}' app to '{_args}'");
        }

        private void ExecuteSetServiceArgs()
        {
            var svcName = AppOrServiceName;
            var svcInfo = Context.Configuration.GetServiceInfo(svcName);
            var args = Context.Configuration.GetServiceArgs(svcName);
            if (args != null && !_force)
            {
                throw new ToolingException($"'{svcName}' {svcInfo.ServiceType} service args already set to '{args}'");
            }

            Context.Configuration.SetServiceArgs(svcName, _args);
            Context.Console.WriteLine($"Set '{svcName}' {svcInfo.ServiceType} service args to '{_args}'");
        }

        private void ExecuteSetServiceTargetArgs()
        {
            var svcName = AppOrServiceName;
            var svcInfo = Context.Configuration.GetServiceInfo(svcName);
            var args = Context.Configuration.GetServiceArgs(svcName, _target);
            if (args != null && !_force)
            {
                throw new ToolingException(
                    $"'{_target}' deploy args for '{AppOrServiceName}' {svcInfo.ServiceType} service already set to '{args}'");
            }

            Context.Configuration.SetServiceArgs(AppOrServiceName, _target, _args);
            Context.Console.WriteLine(
                $"Set '{_target}' deploy args for '{AppOrServiceName}' {svcInfo.ServiceType} service to '{_args}'");
        }
    }
}
