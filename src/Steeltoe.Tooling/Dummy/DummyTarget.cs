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

using System.IO;

namespace Steeltoe.Tooling.Dummy
{
    internal class DummyTarget : Target
    {
        internal DummyTarget(TargetConfiguration configuration) : base(configuration)
        {
        }

        public override IDriver GetDriver(Context context)
        {
            return new DummyDriver(Path.Combine(context.ProjectDirectory, "dummy-service-driver.db"));
        }

        public override bool IsHealthy(Context context)
        {
            context.Console.WriteLine("dummy tool version ... 0.0.0");
            return true;
        }
    }
}
