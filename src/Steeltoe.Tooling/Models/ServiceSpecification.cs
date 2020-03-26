// Copyright 2020 the original author or authors.
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
    /// A model of a service specification.
    /// </summary>
    public class ServiceSpecification
    {
        /// <summary>
        /// Service type.
        /// </summary>
        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Docker image.
        /// </summary>
        [YamlMember(Alias = "image")]
        public string Image { get; set; }

        /// <summary>
        /// Service port.
        /// </summary>
        [YamlMember(Alias = "port")]
        public int Port { get; set; }

        /// <summary>
        /// Nugets.
        /// </summary>
        [YamlMember(Alias = "nugets")]
        public List<string> NuGets { get; set; }
    }
}
