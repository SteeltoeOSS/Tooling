﻿// Copyright 2018 the original author or authors.
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

namespace Steeltoe.Tooling.Executor
{
    public class AddServiceExecutor : IExecutor
    {
        private string Name { get; }

        private string Type { get; }

        public AddServiceExecutor(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public void Execute(Context context)
        {
            context.ServiceManager.AddService(Name, Type);
            context.ServiceManager.EnableService(Name);
            context.Shell.Console.WriteLine($"Added {Type} service '{Name}'");
        }
    }
}