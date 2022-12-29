using Blazored.LocalStorage;
using System.Text.Json;

namespace Obaki.LocalStorageCache.Test.TestHelper
{
    internal class InMemoryStorageCache : ILocalStorageService
    {
        private readonly Dictionary<string, string> _memoryStore = new Dictionary<string, string>();

        #region Not Implemented
        public event EventHandler<ChangingEventArgs> Changing;
        public event EventHandler<ChangedEventArgs> Changed;

        public ValueTask ClearAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<string> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        #endregion

        public ValueTask<T> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            if (IsItemExist(key))
            {
                var data = _memoryStore[key];
                var deserializedData = JsonSerializer.Deserialize<T>(data);
                return new ValueTask<T>(deserializedData ?? default(T));
            }

            return new ValueTask<T>(default(T));
        }

        public ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
        {
            _memoryStore.Remove(key);
            return new ValueTask(Task.CompletedTask);
        }

        public void SetItem(string key, string data)
        {
            if (_memoryStore.ContainsKey(key))
            {
                _memoryStore[key] = data;
            }
            else
            {
                _memoryStore.Add(key, data);
            }
        }

        public ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
        {
            var serializedData = JsonSerializer.Serialize(data);
            SetItem(key, serializedData);

            return new ValueTask(Task.CompletedTask);
        }

        public bool IsItemExist(string key)
        {
            return _memoryStore.ContainsKey(key);
        }
    }
}
