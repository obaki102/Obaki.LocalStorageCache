using Obaki.LocalStorageCache.Test.TestHelper;

namespace Obaki.LocalStorageCache.Test
{
    public class GetCacheValue
    {
        private readonly InMemoryStorageCache _storageCache;
        private readonly LocalStorageCacheProvider _localStorageCache;
        public const string Key = "TestKey";

        public GetCacheValue()
        {
            _storageCache = new InMemoryStorageCache();
            _localStorageCache = new LocalStorageCacheProvider(_storageCache);
        }


        [Fact]
        public async Task GetCacheValue_ValidKey_DataShouldBeRetrieved()
        {
            //Arrange
            var valueSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheValue(Key, valueSaved);

            //Act
            var dataRetrieved = await _localStorageCache.GetCacheValue<DummyObject>(Key);

            //Assert
            Assert.Equal(valueSaved.Id, dataRetrieved.Id);
            Assert.Equal(valueSaved.Name, dataRetrieved.Name);
        }

        [Fact]
        public async Task GetCacheValue_EmptyKey_ShouldThrowAnError()
        {
            //Arrange
            var valueSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheValue(Key, valueSaved);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.GetCacheValue<DummyObject>(string.Empty));

            //Assert
           await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task GetCacheValue_NonExistingKey_ShouldReturnNull()
        {
            //Arrange
            string nonExistingKey = "InvalidKey";
            var valueSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheValue(Key, valueSaved);

            //Act
            var dataRetrieved = await _localStorageCache.GetCacheValue<DummyObject>(nonExistingKey);

            //Assert
            Assert.Null(dataRetrieved);
        }
    }
}
