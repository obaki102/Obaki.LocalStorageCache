using Obaki.LocalStorageCache.Test.TestHelper;


namespace Obaki.LocalStorageCache.Test
{
    public class TryGetCacheAsync
    {
        private readonly InMemoryStorageCache _storageCache;
        private readonly LocalStorageCacheProvider _localStorageCache;
        public const string Key = "TestKey";

        public TryGetCacheAsync()
        {
            _storageCache = new InMemoryStorageCache();
            _localStorageCache = new LocalStorageCacheProvider(_storageCache);
        }

        [Fact]
        public async Task TryGetCacheAsync_ValidValueEntered_CacheDataShouldBeReturned()
        {
            //Arrange
            var valueSaved = new DummyObject(1, "Test");
            _localStorageCache.SetExpiration(TimeSpan.FromHours(1));
            await _localStorageCache.SetCacheAsync(Key, valueSaved);

            //Act
            var valueFromStorage = await _localStorageCache.TryGetCacheAsync<DummyObject>(Key);

            //Assert
            Assert.True(valueFromStorage.isCacheExist);
            Assert.Equal(valueSaved.Id, valueFromStorage.cacheData.Id);
            Assert.Equal(valueSaved.Name, valueFromStorage.cacheData.Name);
        }

        [Fact]
        public async Task TryGetCacheAsync_ExpiredCache_CacheDataShouldReturnFalse()
        {
            //Arrange
            var valueSaved = new DummyObject(1, "Test");
            _localStorageCache.SetExpiration(TimeSpan.FromHours(-1));
            await _localStorageCache.SetCacheAsync(Key, valueSaved);

            //Act
            var valueFromStorage = await _localStorageCache.TryGetCacheAsync<DummyObject>(Key);

            //Assert
            Assert.False(valueFromStorage.isCacheExist);
            Assert.Null(valueFromStorage.cacheData);
            
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
        public async Task TryGetCacheAsync_NonExistingKey_ShouldReturnNull()
        {
            //Arrange
            string nonExistingKey = "InvalidKey";
            var valueSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheAsync(Key, valueSaved);

            //Act
            var dataRetrieved = await _localStorageCache.TryGetCacheAsync<DummyObject>(nonExistingKey);

            //Assert
            Assert.False(dataRetrieved.isCacheExist);
            Assert.Null(dataRetrieved.cacheData);
        }
    }
}
