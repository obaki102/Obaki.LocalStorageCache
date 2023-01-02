using Obaki.LocalStorageCache.Test.TestHelper;

namespace Obaki.LocalStorageCache.Test
{
    public class ClearCache
    {
        private readonly InMemoryStorageCacheSync _storageCache;
        private readonly LocalStorageCacheServiceSync _localStorageCache;
        public const string Key = "TestKey";

        public ClearCache()
        {
            _storageCache = new InMemoryStorageCacheSync();
            _localStorageCache = new LocalStorageCacheServiceSync(_storageCache);
        }

        [Fact]
        public void ClearCache_ValidKey_CacheShouldBeNull()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            _localStorageCache.ClearCache(Key);
            var cacheRetrieved = _localStorageCache.GetCache<DummyObject>(Key);

            //Assert
            Assert.Null(cacheRetrieved);
        }

        [Fact]
        public void ClearCache_NonExisting_CacheShouldBeCleared()
        {
            //Arrange
            string nonExistingKey = "NonExistingKey";
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            _localStorageCache.ClearCache(nonExistingKey);
            var cacheRetrieved = _localStorageCache.GetCache<DummyObject>(Key);

            //Assert
            Assert.Equal(cacheSaved.Id, cacheRetrieved.Id);
            Assert.Equal(cacheSaved.Name, cacheRetrieved.Name);
        }

        [Fact]
        public void ClearCache_InvalidKey_ShouldThrowAnError()
        {
            //Arrange
            string emptyKey = string.Empty;

            //Act
            var action = new Action(() => _localStorageCache.ClearCache(emptyKey));

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
