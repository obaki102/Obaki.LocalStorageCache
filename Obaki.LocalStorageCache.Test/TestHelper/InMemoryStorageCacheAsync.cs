using Blazored.LocalStorage;
using System.Text.Json;

namespace Obaki.LocalStorageCache.Test.TestHelper
{
    internal class InMemoryStorageCacheAsync : ILocalStorageService
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
      
        #endregion

        public ValueTask<T> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            if (IsItemExist(key))
            {
                var Cache = _memoryStore[key];
                var deserializedCache = JsonSerializer.Deserialize<T>(Cache);
                return new ValueTask<T>(deserializedCache ?? default(T));
            }

            return new ValueTask<T>(default(T));
        }

        public ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
        {
            _memoryStore.Remove(key);
            return new ValueTask(Task.CompletedTask);
        }

        public void SetItem(string key, string Cache)
        {
            if (_memoryStore.ContainsKey(key))
            {
                _memoryStore[key] = Cache;
            }
            else
            {
                _memoryStore.Add(key, Cache);
            }
        }

        public ValueTask SetItemAsync<T>(string key, T Cache, CancellationToken cancellationToken = default)
        {
            var serializedCache = JsonSerializer.Serialize(Cache);
            SetItem(key, serializedCache);

            return new ValueTask(Task.CompletedTask);
        }

        public bool IsItemExist(string key)
        {
            return _memoryStore.ContainsKey(key);
        }

        public ValueTask SetItemAsStringAsync(string key, string Cache, CancellationToken cancellationToken = default)
        {
            SetItem(key, Cache);

            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask<string> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
        {
            if (IsItemExist(key))
            {
                var Cache = ReadRawString(key);
                return ValueTask.FromResult(Cache);
            }

            return ValueTask.FromResult(string.Empty);
        }
        public string ReadRawString(string key)
        {
            return _memoryStore[key];
        }
    }
}
