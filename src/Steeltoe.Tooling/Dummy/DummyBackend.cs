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

namespace Steeltoe.Tooling.Dummy
{
    public class DummyBackend : IBackend
    {
        private readonly string _path;

        private DummyServiceDatabase _database;

        public DummyBackend(string path)
        {
            _path = path;
            _database = DummyServiceDatabase.Load(_path);
        }

        public void DeployApp(string app)
        {
            throw new System.NotImplementedException();
        }

        public void UndeployApp(string app)
        {
            throw new System.NotImplementedException();
        }

        public Lifecycle.Status GetAppStatus(string app)
        {
            return GetStatus(app);
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
            return GetStatus(service);
        }

        private Lifecycle.Status GetStatus(string name)
        {
            if (!_database.Services.ContainsKey(name))
            {
                return Lifecycle.Status.Offline;
            }

            var state = _database.Services[name];
            switch (state)
            {
                case Lifecycle.Status.Starting:
                    _database.Services[name] = Lifecycle.Status.Online;
                    Store();
                    break;
                case Lifecycle.Status.Stopping:
                    _database.Services.Remove(name);
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
