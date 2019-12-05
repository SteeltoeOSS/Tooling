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

namespace Steeltoe.Tooling.Drivers.Dummy
{
    internal class DummyDriver : IDriver
    {
        private readonly string _path;

        private DummyServiceDatabase _database;

        internal DummyDriver(string path)
        {
            _path = path;
            _database = DummyServiceDatabase.Load(_path);
        }

        public void DeploySetup()
        {
        }

        public void DeployTeardown()
        {
        }

        public void DeployApp(string app)
        {
            _database.Apps.Add(app);
            Store();
        }

        public void UndeployApp(string app)
        {
            _database.Apps.Remove(app);
            Store();
        }

        public Lifecycle.Status GetAppStatus(string app)
        {
            return _database.Apps.Contains(app) ? Lifecycle.Status.Online : Lifecycle.Status.Offline;
        }

        public void DeployService(string service)
        {
            _database.Services[service] = Lifecycle.Status.Starting;
            Store();
        }

        public void UndeployService(string service)
        {
            _database.Services[service] = Lifecycle.Status.Stopping;
            Store();
        }

        public Lifecycle.Status GetServiceStatus(string service)
        {
            if (!_database.Services.ContainsKey(service))
            {
                return Lifecycle.Status.Offline;
            }

            var state = _database.Services[service];
            switch (state)
            {
                case Lifecycle.Status.Starting:
                    _database.Services[service] = Lifecycle.Status.Online;
                    Store();
                    break;
                case Lifecycle.Status.Stopping:
                    _database.Services.Remove(service);
                    Store();
                    break;
            }

            return state;
        }

        private void Store()
        {
            DummyServiceDatabase.Store(_path, _database);
        }
    }
}
