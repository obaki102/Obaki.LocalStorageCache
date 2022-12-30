
using Microsoft.AspNetCore.DataProtection;
using Moq;
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
            var mockDataProtector = new Mock<IDataProtector>();
            _localStorageCache = new LocalStorageCacheProvider(_storageCache, mockDataProtector.Object);
        }


        [Fact]
        public async Task SetCacheAsync_ValidValueEntered_DataShouldBeSaved()
        {
            //Arrange
            var cacheToSave = new DummyObject(1, "Test");

            //Act
            await _localStorageCache.SetCacheAsync(Key, cacheToSave);
           
            //Assert
            var cacheFromStorage = await _localStorageCache.GetCacheAsync<DummyObject>(Key);
            Assert.Equal(cacheToSave.Id, cacheFromStorage.Id);
            Assert.Equal(cacheToSave.Name, cacheFromStorage.Name);
        }

        [Fact]
        public async Task SetCacheAsync_ExistingValueWithSameKey_DataShouldBeOverwritten()
        {
            //Arrange
            var cacheToSave = new DummyObject(1, "Test");
            var cacheToSave2 = new DummyObject(2, "Test2");

            //Act
            await _localStorageCache.SetCacheAsync(Key, cacheToSave);
            await _localStorageCache.SetCacheAsync(Key, cacheToSave2);

            //Assert
            var cacheFromStorage = _localStorageCache.GetCacheAsync<DummyObject>(Key).Result;
            Assert.Equal(cacheToSave2.Id, cacheFromStorage.Id);
            Assert.Equal(cacheToSave2.Name, cacheFromStorage.Name);
        }

        [Fact]
        public void SetCacheAsync_EmptyKey_ShouldThrowAnError()
        {
            //Arrange
            var cacheToSave = new DummyObject(1, "Test");

            //Act
            var function = new Func<Task>(async () =>  await _localStorageCache.SetCacheAsync(string.Empty, cacheToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public void SetCacheAsync_EmptyData_ShouldThrowAnError()
        {
            //Arrange
            var cacheToSave = default(DummyObject);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.SetCacheAsync(Key, cacheToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public void SetCacheAsync_EmptyKeyAndData_ShouldThrowAnError()
        {
            //Arrange
            var cacheToSave = default(DummyObject);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.SetCacheAsync(string.Empty, cacheToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }
    }
}