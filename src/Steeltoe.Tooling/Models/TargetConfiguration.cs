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

namespace Steeltoe.Tooling.Models
{
    /// <summary>
    /// Represents the configuration of a deployment target.
    /// </summary>
    public class TargetConfiguration
    {
        /// <summary>
        /// Deployment target name.
        /// </summary>
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Deployment target description.
        /// </summary>
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Deployment target properties.
        /// </summary>
        [YamlMember(Alias = "properties")]
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Deployment target application service type properties.
        /// </summary>
        [YamlMember(Alias = "serviceTypeProperties")]
        public Dictionary<string, Dictionary<string, string>> ServiceTypeProperties { get; set; } =
            new Dictionary<string, Dictionary<string, string>>();
    }
}
