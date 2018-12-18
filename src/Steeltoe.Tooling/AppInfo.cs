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
    /// <summary>
    /// An application that can be deployed on a managed backend.
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// Application name.
        /// </summary>
        public string App { get; }

        /// <summary>
        /// Creates a new AppInfo for the specifed applicaition.
        /// </summary>
        /// <param name="app">Application name.</param>
        public AppInfo(string app)
        {
            App = app;
        }
    }
}
