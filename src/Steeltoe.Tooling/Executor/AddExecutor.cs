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

namespace Steeltoe.Tooling.Executor
{
    [RequiresInitialization]
    public class AddExecutor : Executor
    {
        private readonly string _itemName;

        private readonly string _itemType;

        public AddExecutor(string itemName, string itemType)
        {
            _itemName = itemName;
            _itemType = itemType;
        }

        protected override void Execute()
        {
            if (_itemType == "app")
            {
                Context.Configuration.AddApp(_itemName);
                Context.Console.WriteLine($"Added app '{_itemName}'");
            }
            else
            {
                Context.Configuration.AddService(_itemName, _itemType);
                Context.Console.WriteLine($"Added {_itemType} service '{_itemName}'");
            }
        }
    }
}
