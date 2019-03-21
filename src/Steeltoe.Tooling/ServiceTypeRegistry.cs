﻿// Copyright 2018 the original author or authors.
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
using System.Linq;

namespace Steeltoe.Tooling
{
    public static class ServiceTypeRegistry
    {
        private static SortedDictionary<string, ServiceType> _types = new SortedDictionary<string, ServiceType>();

        static ServiceTypeRegistry()
        {
            Register(new ServiceType("config-server", 8888, "Cloud Foundry Config Server"));
            Register(new ServiceType("eureka", 8761, "Netflix Eureka Server"));
            Register(new ServiceType("hystrix", 7979, "Netflix Hystrix Server"));
            Register(new ServiceType("mssql", 1433, "Microsoft SQL Server"));
            Register(new ServiceType("redis", 6379, "Redis Server"));
            Register(new ServiceType("uaa", 8080, "Workshop User Account and Authentication Server"));
            Register(new ServiceType("zipkin", 9411, "Zipkin Distributed Tracing System"));
            if (Settings.DummiesEnabled)
            {
                Register(new ServiceType("dummy-svc", 0, "A Dummy Service"));
            }
        }

        public static List<string> Names => _types.Keys.ToList();

        internal static ServiceType ForName(string name)
        {
            try
            {
                return _types[name];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        private static void Register(ServiceType type)
        {
            _types[type.Name] = type;
        }
    }
}
