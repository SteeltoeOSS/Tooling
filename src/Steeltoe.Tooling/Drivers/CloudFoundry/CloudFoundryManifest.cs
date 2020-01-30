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

using System.Collections.Generic;

namespace Steeltoe.Tooling.Drivers.CloudFoundry
{
    internal class CloudFoundryManifest
    {
        public List<Application> Applications { get; set; } = new List<Application>();

        public class Application
        {
            public string Name { get; set; }

            public string Command { get; set; }

            public List<string> BuildPacks { get; set; }

            public string Stack { get; set; }

            public string Memory { get; set; }

            public Dictionary<string, string> Environment { get; set; }

            public List<string> ServiceNames { get; set; }
        }
    }
}
