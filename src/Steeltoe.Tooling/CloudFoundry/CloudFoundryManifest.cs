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
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.CloudFoundry
{
    internal class CloudFoundryManifest
    {
        [YamlMember(Alias = "applications")]
        public List<Application> Applications { get; set; }= new List<Application>();

        public class Application
        {
            [YamlMember(Alias = "name")]
            public string Name { get; set; }

            [YamlMember(Alias = "command")]
            public string Command { get; set; }

            [YamlMember(Alias = "buildpacks")]
            public List<string> BuildPacks { get; set; }

            [YamlMember(Alias = "stack")]
            public string Stack { get; set; }

            [YamlMember(Alias = "memory")]
            public string Memory { get; set; }

            [YamlMember(Alias = "env")]
            public Dictionary<string, string> Environment { get; set; }

            [YamlMember(Alias = "services")]
            public List<string> ServiceNames { get; set; }
        }
    }
}
