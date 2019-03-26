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

using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace Steeltoe.Cli.Test
{
    public class MockConsole : IConsole
    {
        public MockConsole()
        {
            Clear();
        }

        public void ResetColor()
        {
        }

        public TextWriter Out { get; private set; }
        public TextWriter Error { get; private set; }
        public TextReader In { get; }
        public bool IsInputRedirected { get; }
        public bool IsOutputRedirected { get; }
        public bool IsErrorRedirected { get; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add { }
            remove { }
        }

        public void Clear()
        {
            Out = new StringWriter();
            Error = new StringWriter();
        }
    }
}
