using Obaki.LocalStorageCache.Test.TestHelper;

namespace Obaki.LocalStorageCache.Test
{
    public class TryGetCache
    {
        private readonly InMemoryStorageCacheSync _storageCache;
        private readonly LocalStorageCacheServiceSync _localStorageCache;
        public const string Key = "TestKey";

        public TryGetCache()
        {
            _storageCache = new InMemoryStorageCacheSync();
            _localStorageCache = new LocalStorageCacheServiceSync(_storageCache);
        }

        [Fact]
        public void TryGetCachec_ValidValueEntered_CacheShouldBeReturned()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.CacheExpiration = TimeSpan.FromHours(1);
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            var isCacheExist = _localStorageCache.TryGetCache(Key, out DummyObject? cacheData);

            //Assert
            Assert.True(isCacheExist);
            Assert.Equal(cacheSaved.Id, cacheData.Id);
            Assert.Equal(cacheSaved.Name, cacheData.Name);
        }

        [Fact]
        public void TryGetCachec_ExpiredCache_CacheCacheShouldReturnFalseAndCacheIsNull()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.CacheExpiration = TimeSpan.FromHours(-1);
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            var isCacheExist = _localStorageCache.TryGetCache(Key, out DummyObject? cacheData);

            //Assert
            Assert.False(isCacheExist);
            Assert.Null(cacheData);

        }

        [Fact]
        public void TryGetCachec_EmptyKey_ShouldThrowAnError()
        {
            //Act
            var action = new Action(() => _localStorageCache.TryGetCache(string.Empty, out DummyObject? cacheData));

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TryGetCachec_NonExistingKey_ShouldReturnFalseAndNull()
        {
            //Arrange
            string nonExistingKey = "EmptyKey";
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.CacheExpiration = TimeSpan.FromHours(1);
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            var isCacheExist = _localStorageCache.TryGetCache(nonExistingKey, out DummyObject? cacheData);

            //Assert
            Assert.False(isCacheExist);
            Assert.Null(cacheData);
        }
    }
}
