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
        public void TryGetCachec_ValidValueEntered_CacheCacheShouldBeReturned()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.CacheExpiration = TimeSpan.FromHours(1);
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            var (isCacheExist, cacheCache) = _localStorageCache.TryGetCache<DummyObject>(Key);

            //Assert
            Assert.True(isCacheExist);
            Assert.Equal(cacheSaved.Id, cacheCache.Id);
            Assert.Equal(cacheSaved.Name, cacheCache.Name);
        }

        [Fact]
        public void TryGetCachec_ExpiredCache_CacheCacheShouldReturnFalseAndCacheCacheIsNull()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.CacheExpiration = TimeSpan.FromHours(-1);
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            var (isCacheExist, cacheCache) = _localStorageCache.TryGetCache<DummyObject>(Key);

            //Assert
            Assert.False(isCacheExist);
            Assert.Null(cacheCache);

        }

        [Fact]
        public void TryGetCachec_EmptyKey_ShouldThrowAnError()
        {
            //Act
            var action = new Action(() => _localStorageCache.TryGetCache<DummyObject>(string.Empty));

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void TryGetCachec_NonExistingKey_ShouldReturnFalseAndNull()
        {
            //Arrange
            string nonExistingKey = "EmptyKey";
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            var (isCacheExist, cacheCache) = _localStorageCache.TryGetCache<DummyObject>(nonExistingKey);

            //Assert
            Assert.False(isCacheExist);
            Assert.Null(cacheCache);
        }
    }
}
