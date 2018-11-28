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

using System.IO;

namespace Steeltoe.Tooling
{
    public static class Settings
    {
        public static bool DebugEnabled { get; set; }

        public static bool VerboseEnabled { get; set; }

        public static bool DummiesEnabled { get; set; }

        public static int MaxChecks { get; set; } = -1;

        static Settings()
        {
            DummiesEnabled = File.Exists(".steeltoe.dummies");
        }
    }
}
