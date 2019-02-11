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

using System.Collections.Generic;
using System.IO;
using System.Threading;
using YamlDotNet.Serialization;

namespace Steeltoe.Tooling.Dummy
{
    internal class DummyServiceDatabase
    {
        public List<string> Apps { get; set; } = new List<string>();

        public SortedDictionary<string, Lifecycle.Status> Services { get; set; } =
            new SortedDictionary<string, Lifecycle.Status>();

        internal static void Store(string path, DummyServiceDatabase database)
        {
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(database);
            using (var fOut = GetFileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fOut))
                {
                    writer.Write(yaml);
                }
            }
        }

        internal static DummyServiceDatabase Load(string path)
        {
            var deserializer = new DeserializerBuilder().Build();
            using (var fIn = GetFileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (var reader = new StreamReader(fIn))
                {
                    return deserializer.Deserialize<DummyServiceDatabase>(reader) ??
                           new DummyServiceDatabase();
                }
            }
        }

        private static FileStream GetFileStream(string path, FileMode mode, FileAccess access)
        {
            while (true)
            {
                try
                {
                    return new FileStream(path, mode, access, FileShare.None);
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
