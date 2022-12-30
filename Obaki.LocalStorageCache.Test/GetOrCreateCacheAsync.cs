using Obaki.LocalStorageCache.Test.TestHelper;
using System.Xml.Linq;

namespace Obaki.LocalStorageCache.Test
{
    public class GetOrCreateCacheAsync
    {
        private readonly InMemoryStorageCache _storageCache;
        private readonly LocalStorageCacheProvider _localStorageCache;
        public const string Key = "TestKey";

        public GetOrCreateCacheAsync()
        {
            _storageCache = new InMemoryStorageCache();
            _localStorageCache = new LocalStorageCacheProvider(_storageCache);
        }


        [Theory]
        [InlineData(1, "Test")]
        [InlineData(2, "Test2")]
        [InlineData(3, "Test3")]
        [InlineData(4, "Test4")]
        public async Task GetOrCreateCacheAsync_ValidKey_DataShouldBeRetrievedFromCache(int id, string name)
        {
            //Arrange
            var cacheSaved = new DummyObject(id, name);
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);

            //Act
            var cacheRetrieved = await _localStorageCache.GetOrCreateCacheAsync(
                  Key,
                   TimeSpan.FromHours(1),
                  async () =>
                  {
                      return await ValueTask.FromResult(new DummyObject(11, "Test11"));
                  });

            //Assert
            Assert.Equal(cacheSaved.Id, cacheRetrieved.Id);
            Assert.Equal(cacheSaved.Name, cacheRetrieved.Name);
        }

        [Fact]
        public async Task GetOrCreateCacheAsync_CacheExpired_NewCacheShouldBeReturned()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);
            var newCache = new DummyObject(2, "Test2");

            //Act
            var cacheRetrieved = await _localStorageCache.GetOrCreateCacheAsync(
                  Key,
                  TimeSpan.FromHours(-1),
                  async () =>
                  {
                      return await ValueTask.FromResult(newCache);
                  });

            //Assert
            Assert.Equal(newCache.Id, cacheRetrieved.Id);
            Assert.Equal(newCache.Name, cacheRetrieved.Name);
        }

        [Fact]
        public async Task GetOrCreateCacheAsync_FirstTimeCacheIsCreated_NewCacheShouldBeReturned()
        {
            //Arrange
            var newCache = new DummyObject(2, "Test2");

            //Act
            var cacheRetrieved = await _localStorageCache.GetOrCreateCacheAsync(
                  Key,
                  TimeSpan.FromHours(1),
                  async () =>
                  {
                      return await ValueTask.FromResult(newCache);
                  });

            //Assert
            Assert.Equal(newCache.Id, cacheRetrieved.Id);
            Assert.Equal(newCache.Name, cacheRetrieved.Name);
        }

        [Fact]
        public async Task GetOrCreateCacheAsync_EmptyKey_ErrorShouldBeThrown()
        {
            //Arrange
            var newCache = new DummyObject(2, "Test2");

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.GetOrCreateCacheAsync(
                  string.Empty,
                  TimeSpan.FromHours(1),
                  async () =>
                  {
                      return await ValueTask.FromResult(newCache);
                  }));

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task GetOrCreateCacheAsync_NonExistingKey_CreateNewCache()
        {
            //Arrange
            string nonExsitingKey = "NonExsitingKey";
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);
            var newCache = new DummyObject(2, "Test2");

            //Act
            var cacheRetrieved = await _localStorageCache.GetOrCreateCacheAsync(
                   nonExsitingKey,
                   TimeSpan.FromHours(1),
                   async () =>
                   {
                       return await ValueTask.FromResult(newCache);
                   });


            //Assert
            Assert.NotNull(cacheRetrieved);
            Assert.Equal(newCache.Id, cacheRetrieved.Id);
            Assert.Equal(newCache.Name, cacheRetrieved.Name);
        }
    }
}
