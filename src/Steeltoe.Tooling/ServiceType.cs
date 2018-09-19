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

namespace Steeltoe.Tooling
{
    public class ServiceType
    {
        public string Name { get; }

        public int Port { get; }

        public string Description { get; }

        public ServiceType(string name, int port, string description)
        {
            Name = name;
            Port = port;
            Description = description;
        }

        public override string ToString()
        {
            return $"{Name} ({Description})";
        }
    }
}
