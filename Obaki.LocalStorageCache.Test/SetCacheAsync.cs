
using Obaki.LocalStorageCache.Test.TestHelper;

namespace Obaki.LocalStorageCache.Test
{
    public class SetCacheAsync
    {
        private readonly InMemoryStorageCache _storageCache;
        private readonly LocalStorageCacheProvider _localStorageCache;
        public const string Key = "TestKey";

        public SetCacheAsync()
        {
            _storageCache = new InMemoryStorageCache();
            _localStorageCache = new LocalStorageCacheProvider(_storageCache);
        }


        [Fact]
        public async Task SetCacheAsync_ValidValueEntered_DataShouldBeSaved()
        {
            //Arrange
            var valueToSave = new DummyObject(1, "Test");

            //Act
            await _localStorageCache.SetCacheAsync(Key, valueToSave);
           
            //Assert
            var valueFromStorage = await _localStorageCache.GetCacheAsync<DummyObject>(Key);
            Assert.Equal(valueToSave.Id, valueFromStorage.Id);
            Assert.Equal(valueToSave.Name, valueFromStorage.Name);
        }

        [Fact]
        public async Task SetCacheAsync_ExistingValueWithSameKey_DataShouldBeOverwritten()
        {
            //Arrange
            var valueToSave = new DummyObject(1, "Test");
            var valueToSave2 = new DummyObject(2, "Test2");

            //Act
            await _localStorageCache.SetCacheAsync(Key, valueToSave);
            await _localStorageCache.SetCacheAsync(Key, valueToSave2);

            //Assert
            var valueFromStorage = _localStorageCache.GetCacheAsync<DummyObject>(Key).Result;
            Assert.Equal(valueToSave2.Id, valueFromStorage.Id);
            Assert.Equal(valueToSave2.Name, valueFromStorage.Name);
        }

        [Fact]
        public void SetCacheAsync_EmptyKey_ShouldThrowAnError()
        {
            //Arrange
            var valueToSave = new DummyObject(1, "Test");

            //Act
            var function = new Func<Task>(async () =>  await _localStorageCache.SetCacheAsync(string.Empty, valueToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public void SetCacheAsync_EmptyData_ShouldThrowAnError()
        {
            //Arrange
            var valueToSave = default(DummyObject);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.SetCacheAsync(Key, valueToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public void SetCacheAsync_EmptyKeyAndData_ShouldThrowAnError()
        {
            //Arrange
            var valueToSave = default(DummyObject);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.SetCacheAsync(string.Empty, valueToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }
    }
}