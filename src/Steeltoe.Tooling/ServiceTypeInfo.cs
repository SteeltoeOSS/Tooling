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

using YamlDotNet.Serialization;

namespace Steeltoe.Tooling
{
    /// <summary>
    /// Specifies a service.
    /// </summary>
    public class ServiceTypeInfo
    {
        /// <summary>
        /// Service name.
        /// </summary>
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Service port.
        /// </summary>
        [YamlMember(Alias = "port")]
        public int Port { get; set; }

        /// <summary>
        /// Service description.
        /// </summary>
        [YamlMember(Alias = "description")]
        public string Description { get; set;  }

        /// <summary>
        /// Returns a human-readable representation of this ServiceTypeInfo.
        /// </summary>
        /// <returns>A human readable string.</returns>
        public override string ToString()
        {
            return $"ServiceTypeinfo[name={Name},port={Port},desc=\"{Description}\"]";
        }
    }
}
