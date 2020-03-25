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

using System.IO;

namespace Steeltoe.Tooling
{
    /// <summary>
    /// Represents the context in which Steeltoe Tooling operations are performed.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Steeltoe Tooling working directory.
        /// </summary>
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// Steeltoe Tooling console.  Typically the console is used to display messages to the user.
        /// </summary>
        public TextWriter Console { get; set; }

        /// <summary>
        /// Shell which which to run system commands.
        /// </summary>
        public Shell Shell { get; set; }

        /// <summary>
        /// Tooling registry.
        /// </summary>
        public Registry Registry { get; set; }
    }
}
