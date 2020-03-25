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

using System;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.Models
{
    /// <summary>
    /// A network port.
    /// </summary>
    public class Service : IComparable
    {
        /// <summary>
        /// Service protocol.
        /// </summary>
        [YamlMember(Alias = "protocol")]
        public string Protocol { get; }

        /// <summary>
        /// Service port.
        /// </summary>
        [YamlMember(Alias = "port")]
        public int Port { get; }

        /// <summary>
        /// Create a new service.
        /// </summary>
        /// <param name="protocol">Service protocol.</param>
        /// <param name="port">Service port.</param>
        public Service(string protocol, int port)
        {
            Protocol = protocol;
            Port = port;
        }


        /// <summary>
        /// Returns a comparison of the Services' protocols, and if equal, a comparison of the ports.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            var svc = (Service) obj;
            var compare = Protocol.CompareTo(svc.Protocol);
            if (compare != 0)
            {
                return compare;
            }

            return Port.CompareTo(svc.Port);
        }
    }
}
