using Obaki.LocalStorageCache.Test.TestHelper;


namespace Obaki.LocalStorageCache.Test
{
    public class TryGetCacheAsync
    {
        private readonly InMemoryStorageCacheAsync _storageCache;
        private readonly LocalStorageCacheServiceAsync _localStorageCache;
        public const string Key = "TestKey";

        public TryGetCacheAsync()
        {
            _storageCache = new InMemoryStorageCacheAsync();
            _localStorageCache = new LocalStorageCacheServiceAsync(_storageCache);
        }

        [Fact]
        public async Task TryGetCacheAsync_ValidValueEntered_CacheShouldBeReturned()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.CacheExpiration = TimeSpan.FromHours(1);
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);

            //Act
            var (isCacheExist, cacheCache) = await _localStorageCache.TryGetCacheAsync<DummyObject>(Key);

            //Assert
            Assert.True(isCacheExist);
            Assert.Equal(cacheSaved.Id, cacheCache.Id);
            Assert.Equal(cacheSaved.Name, cacheCache.Name);
        }

        [Fact]
        public async Task TryGetCacheAsync_ExpiredCache_CacheCacheShouldReturnFalseAndCacheIsNull()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.CacheExpiration = TimeSpan.FromHours(-1);
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);

            //Act
            var (isCacheExist, cacheData) = await _localStorageCache.TryGetCacheAsync<DummyObject>(Key);

            //Assert
            Assert.False(isCacheExist);
            Assert.Null(cacheData);
            
        }

        [Fact]
        public void TryGetCacheAsync_EmptyKey_ShouldThrowAnError()
        {
            //Act
            var function = new Func<Task>(async () => await _localStorageCache.TryGetCacheAsync<DummyObject>(string.Empty));

            //Assert
             Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task TryGetCacheAsync_NonExistingKey_ShouldReturnFalseAndNull()
        {
            //Arrange
            string nonExistingKey = "EmptyKey";
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.CacheExpiration = TimeSpan.FromHours(1);
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);

            //Act
            var (isCacheExist, cacheData) = await _localStorageCache.TryGetCacheAsync<DummyObject>(nonExistingKey);

            //Assert
            Assert.False(isCacheExist);
            Assert.Null(cacheData);
        }
    }
}
