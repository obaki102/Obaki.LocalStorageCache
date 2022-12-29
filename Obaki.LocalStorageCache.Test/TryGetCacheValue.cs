using Obaki.LocalStorageCache.Test.TestHelper;


namespace Obaki.LocalStorageCache.Test
{
    public class TryGetCacheValue
    {
        private readonly InMemoryStorageCache _storageCache;
        private readonly LocalStorageCacheProvider _localStorageCache;
        public const string Key = "TestKey";

        public TryGetCacheValue()
        {
            _storageCache = new InMemoryStorageCache();
            _localStorageCache = new LocalStorageCacheProvider(_storageCache);
        }

        [Fact]
        public async Task TryGetCacheValue_ValidValueEntered_CacheDataShouldBeReturned()
        {
            //Arrange
            var valueSaved = new DummyObject(1, "Test");
            _localStorageCache.SetExpiration(TimeSpan.FromHours(1));
            await _localStorageCache.SetCacheValue(Key, valueSaved);

            //Act
            var valueFromStorage = await _localStorageCache.TryGetCacheValue<DummyObject>(Key);

            //Assert
            Assert.True(valueFromStorage.isCacheExist);
            Assert.Equal(valueSaved.Id, valueFromStorage.cacheData.Id);
            Assert.Equal(valueSaved.Name, valueFromStorage.cacheData.Name);
        }

        [Fact]
        public async Task TryGetCacheValue_ExpiredCache_CacheDataShouldReturnFalse()
        {
            //Arrange
            var valueSaved = new DummyObject(1, "Test");
            _localStorageCache.SetExpiration(TimeSpan.FromHours(-1));
            await _localStorageCache.SetCacheValue(Key, valueSaved);

            //Act
            var valueFromStorage = await _localStorageCache.TryGetCacheValue<DummyObject>(Key);

            //Assert
            Assert.False(valueFromStorage.isCacheExist);
            Assert.Null(valueFromStorage.cacheData);
            
        }

        [Fact]
        public void TryGetCacheValue_EmptyKey_ShouldThrowAnError()
        {
            //Act
            var function = new Func<Task>(async () => await _localStorageCache.TryGetCacheValue<DummyObject>(string.Empty));

            //Assert
             Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task TryGetCacheValue_NonExistingKey_ShouldReturnNull()
        {
            //Arrange
            string nonExistingKey = "InvalidKey";
            var valueSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheValue(Key, valueSaved);

            //Act
            var dataRetrieved = await _localStorageCache.TryGetCacheValue<DummyObject>(nonExistingKey);

            //Assert
            Assert.False(dataRetrieved.isCacheExist);
            Assert.Null(dataRetrieved.cacheData);
        }
    }
}
