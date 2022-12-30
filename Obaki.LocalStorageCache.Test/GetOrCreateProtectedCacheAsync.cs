using Obaki.LocalStorageCache.Test.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obaki.LocalStorageCache.Test
{
    public class GetOrCreateProtectedCacheAsync
    {
        private readonly InMemoryStorageCache _storageCache;
        private readonly LocalStorageCacheProvider _localStorageCache;
        private readonly TestDataProtectionProvider _testDataProtectionProvider;
        public const string Key = "TestKey";

        public GetOrCreateProtectedCacheAsync()
        {
            _storageCache = new InMemoryStorageCache();
            _testDataProtectionProvider = new TestDataProtectionProvider();
            _localStorageCache = new LocalStorageCacheProvider(_storageCache, _testDataProtectionProvider);
        }
        [Theory]
        [InlineData(1, "Test")]
        [InlineData(2, "Test2")]
        [InlineData(3, "Test3")]
        [InlineData(4, "Test4")]
        public async Task GetOrCreateProtectedCacheAsync_ValidKey_DataShouldBeRetrievedFromCache(int id, string name)
        {
            //Arrange
            var cacheSaved = new DummyObject(id, name);
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheSaved);

            //Act
            var cacheRetrieved = await _localStorageCache.GetOrCreateProtectedCacheAsync(
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
        public async Task GetOrCreateProtectedCacheAsync_CacheExpired_NewCacheShouldBeReturned()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheSaved);
            var newCache = new DummyObject(2, "Test2");

            //Act
            var cacheRetrieved = await _localStorageCache.GetOrCreateProtectedCacheAsync(
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
        public async Task GetOrCreateProtectedCacheAsync_FirstTimeCacheIsCreated_NewCacheShouldBeReturned()
        {
            //Arrange
            var newCache = new DummyObject(2, "Test2");

            //Act
            var cacheRetrieved = await _localStorageCache.GetOrCreateProtectedCacheAsync(
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
        public async Task GetOrCreateProtectedCacheAsync_EmptyKey_ErrorShouldBeThrown()
        {
            //Arrange
            var newCache = new DummyObject(2, "Test2");

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.GetOrCreateProtectedCacheAsync(
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
        public async Task GetOrCreateProtectedCacheAsync_NonExistingKey_CreateNewCache()
        {
            //Arrange
            string nonExsitingKey = "NonExsitingKey";
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheSaved);
            var newCache = new DummyObject(2, "Test2");

            //Act
            var cacheRetrieved = await _localStorageCache.GetOrCreateProtectedCacheAsync(
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
