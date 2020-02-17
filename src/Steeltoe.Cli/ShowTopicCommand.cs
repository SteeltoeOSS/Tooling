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
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Steeltoe.Tooling;
using Steeltoe.Tooling.Controllers;

namespace Steeltoe.Cli
{
    [Command(Description = "Displays documentation on a topic",
        ExtendedHelpText = @"
Overview:
  Displays documentation for various topics.  Run with no arguments for a list of available topics.

Examples:
  Display documentation for autodetection:
  $ st show-topic autodetection"
    )]
    public class ShowTopicCommand : Command
    {
        public const string CommandName = "show-topic";

        [Argument(0, Name = "topic", Description = "Topic")]
        private string Topic { get; } = null;

        public ShowTopicCommand(IConsole console) : base(console)
        {
        }

        protected override Controller GetController()
        {
            return new ShowTopicController(Topic);
        }

        private class ShowTopicController : Controller
        {
            private static readonly ILogger<ShowTopicController> Logger =
                Logging.LoggerFactory.CreateLogger<ShowTopicController>();

            private readonly string _topicsPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "steeltoe.rc",
                "topics");

            private readonly string _topic;

            public ShowTopicController(string topic)
            {
                _topic = topic;
            }

            protected override void Execute()
            {
                Logger.LogDebug($"topics path: {_topicsPath}");
                if (_topic == null)
                {
                    ListTopics();
                }
                else
                {
                    ShowTopic(_topic);
                }
            }

            private void ListTopics()
            {
                var topicPaths = Directory.GetFiles(_topicsPath, "*.txt");
                var maxTopicLength = topicPaths.Select(Path.GetFileNameWithoutExtension)
                    .Select(topic => topic.Length).Concat(new[] {0}).Max();
                var descriptionColumn = maxTopicLength + 4;
                foreach (var topicPath in topicPaths)
                {
                    var topic = Path.GetFileNameWithoutExtension(topicPath);
                    var description = File.ReadLines(topicPath).First();
                    Context.Console.WriteLine($"{topic.PadRight(descriptionColumn)}{description}");
                }
            }

            private void ShowTopic(string topic)
            {
                var topicPath = Path.Combine(_topicsPath, $"{topic}.txt");
                if (!File.Exists(topicPath))
                {
                    throw new ItemDoesNotExistException(topic);
                }

                foreach (var line in File.ReadLines(topicPath))
                {
                    Context.Console.WriteLine(line);
                }
            }
        }
    }
}
