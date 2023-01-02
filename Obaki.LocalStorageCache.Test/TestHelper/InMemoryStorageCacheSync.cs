using Blazored.LocalStorage;
using System.Text.Json;
namespace Obaki.LocalStorageCache.Test.TestHelper
{
    internal class InMemoryStorageCacheSync : ISyncLocalStorageService
    {
        private readonly Dictionary<string, string> _memoryStore = new Dictionary<string, string>();
        public event EventHandler<ChangingEventArgs> Changing;
        public event EventHandler<ChangedEventArgs> Changed;

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool ContainKey(string key)
        {
            throw new NotImplementedException();
        }

        public T GetItem<T>(string key)
        {
            if (IsItemExist(key))
            {
                var Cache = _memoryStore[key];
                var deserializedCache = JsonSerializer.Deserialize<T>(Cache);
                return deserializedCache ?? default;
            }

            return default;
        }

        public string GetItemAsString(string key)
        {
            throw new NotImplementedException();
        }

        public string Key(int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Keys()
        {
            throw new NotImplementedException();
        }

        public int Length()
        {
            throw new NotImplementedException();
        }

        public void RemoveItem(string key)
        {
            _memoryStore.Remove(key);
        }

        public void RemoveItems(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public void SetItem<T>(string key, T data)
        {
            var serializedCache = JsonSerializer.Serialize(data);
            if (_memoryStore.ContainsKey(key))
            {
                _memoryStore[key] = serializedCache;
            }
            else
            {
                _memoryStore.Add(key, serializedCache);
            }
        }

        public void SetItemAsString(string key, string data)
        {
            throw new NotImplementedException();
        }

        private bool IsItemExist(string key)
        {
            return _memoryStore.ContainsKey(key);
        }

        
    }
}
