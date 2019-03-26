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

namespace Steeltoe.Tooling
{
    /// <summary>
    /// Represents errors that can be thrown by a <see cref="Cli"/>
    /// </summary>
    public class CliException : ToolingException
    {
        /// <summary>
        /// Command return code.
        /// </summary>
        public int ReturnCode { get; }

        /// <summary>
        /// Command that generated the exception.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Command error message.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Creates a new CliException.
        /// </summary>
        /// <param name="returnCode">Command return code.</param>
        /// <param name="command">Command.</param>
        /// <param name="error">Command error message.</param>
        public CliException(int returnCode, string command, string error) : base(
            $"{error} [rc={returnCode},cmd='{command}']")
        {
            ReturnCode = returnCode;
            Command = command;
            Error = error;
        }
    }
}
