using Microsoft.AspNetCore.DataProtection;
using Moq;
using Obaki.LocalStorageCache.Test.TestHelper;

namespace Obaki.LocalStorageCache.Test
{
    public class GetCacheAsync
    {
        private readonly InMemoryStorageCache _storageCache;
        private readonly LocalStorageCacheProvider _localStorageCache;
        public const string Key = "TestKey";

        public GetCacheAsync()
        {
            _storageCache = new InMemoryStorageCache();
            var mockDataProtector = new Mock<IDataProtector>();
            _localStorageCache = new LocalStorageCacheProvider(_storageCache, mockDataProtector.Object);
        }


        [Fact]
        public async Task GetCacheAsync_ValidKey_DataShouldBeRetrieved()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);

            //Act
            var cacheRetrieved = await _localStorageCache.GetCacheAsync<DummyObject>(Key);

            //Assert
            Assert.Equal(cacheSaved.Id, cacheRetrieved.Id);
            Assert.Equal(cacheSaved.Name, cacheRetrieved.Name);
        }

        [Fact]
        public async Task GetCacheAsync_EmptyKey_ShouldThrowAnError()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.GetCacheAsync<DummyObject>(string.Empty));

            //Assert
           await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task GetCacheAsync_NonExistingKey_ShouldReturnNull()
        {
            //Arrange
            string nonExistingKey = "EmptyKey";
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);

            //Act
            var cacheRetrieved = await _localStorageCache.GetCacheAsync<DummyObject>(nonExistingKey);

            //Assert
            Assert.Null(cacheRetrieved);
        }
    }
}
