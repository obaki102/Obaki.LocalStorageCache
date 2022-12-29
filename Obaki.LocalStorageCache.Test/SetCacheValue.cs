
using Obaki.LocalStorageCache.Test.TestHelper;

namespace Obaki.LocalStorageCache.Test
{
    public class SetCacheValue
    {
        private readonly InMemoryStorageCache _storageCache;
        private readonly LocalStorageCacheProvider _localStorageCache;
        public const string Key = "TestKey";

        public SetCacheValue()
        {
            _storageCache = new InMemoryStorageCache();
            _localStorageCache = new LocalStorageCacheProvider(_storageCache);
        }


        [Fact]
        public async Task SetCacheValue_ValidValueEntered_DataShouldBeSaved()
        {
            //Arrange
            var valueToSave = new DummyObject(1, "Test");

            //Act
            await _localStorageCache.SetCacheValue(Key, valueToSave);
           
            //Assert
            var valueFromStorage = await _localStorageCache.GetCacheValue<DummyObject>(Key);
            Assert.Equal(valueToSave.Id, valueFromStorage.Id);
            Assert.Equal(valueToSave.Name, valueFromStorage.Name);
        }

        [Fact]
        public async Task SetCacheValue_ExistingValueWithSameKey_DataShouldBeOverwritten()
        {
            //Arrange
            var valueToSave = new DummyObject(1, "Test");
            var valueToSave2 = new DummyObject(2, "Test2");

            //Act
            await _localStorageCache.SetCacheValue(Key, valueToSave);
            await _localStorageCache.SetCacheValue(Key, valueToSave2);

            //Assert
            var valueFromStorage = _localStorageCache.GetCacheValue<DummyObject>(Key).Result;
            Assert.Equal(valueToSave2.Id, valueFromStorage.Id);
            Assert.Equal(valueToSave2.Name, valueFromStorage.Name);
        }

        [Fact]
        public void SetCacheValue_EmptyKey_ShouldThrowAnError()
        {
            //Arrange
            var valueToSave = new DummyObject(1, "Test");

            //Act
            var function = new Func<Task>(async () =>  await _localStorageCache.SetCacheValue(string.Empty, valueToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public void SetCacheValue_EmptyData_ShouldThrowAnError()
        {
            //Arrange
            var valueToSave = default(DummyObject);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.SetCacheValue(Key, valueToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public void SetCacheValue_EmptyKeyAndData_ShouldThrowAnError()
        {
            //Arrange
            var valueToSave = default(DummyObject);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.SetCacheValue(string.Empty, valueToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }
    }
}