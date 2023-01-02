using Obaki.LocalStorageCache.Test.TestHelper;

namespace Obaki.LocalStorageCache.Test
{
    public class ClearCacheAsync
    {
        private readonly InMemoryStorageCacheAsync _storageCache;
        private readonly LocalStorageCacheServiceAsync _localStorageCache;
        public const string Key = "TestKey";

        public ClearCacheAsync()
        {
            _storageCache = new InMemoryStorageCacheAsync();
            _localStorageCache = new LocalStorageCacheServiceAsync(_storageCache);
        }

        [Fact]
        public async Task ClearCacheAsync_ValidKey_CacheShouldBeNull()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);

            //Act
            await _localStorageCache.ClearCacheAsync(Key);
            var cacheRetrieved = await _localStorageCache.GetCacheAsync<DummyObject>(Key);

            //Assert
            Assert.Null(cacheRetrieved);
        }

        [Fact]
        public async Task ClearCacheAsync_NonExisting_CacheShouldBeCleared()
        {
            //Arrange
            string nonExistingKey = "NonExistingKey";
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);

            //Act
            await _localStorageCache.ClearCacheAsync(nonExistingKey);
            var cacheRetrieved = await _localStorageCache.GetCacheAsync<DummyObject>(Key);

            //Assert
            Assert.Equal(cacheSaved.Id, cacheRetrieved.Id);
            Assert.Equal(cacheSaved.Name, cacheRetrieved.Name);
        }

        [Fact]
        public async Task ClearCacheAsync_InvalidKey_ShouldThrowAnError()
        {
            //Arrange
            string emptyKey = string.Empty;

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.ClearCacheAsync(emptyKey));

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }
    }
}
