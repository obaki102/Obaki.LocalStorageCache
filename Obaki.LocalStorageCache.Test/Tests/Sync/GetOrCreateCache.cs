using Obaki.LocalStorageCache.Test.TestHelper;

namespace Obaki.LocalStorageCache.Test
{
    public class GetOrCreateCache
    {
        private readonly InMemoryStorageCacheSync _storageCache;
        private readonly LocalStorageCacheServiceSync _localStorageCache;
        public const string Key = "TestKey";

        public GetOrCreateCache()
        {
            _storageCache = new InMemoryStorageCacheSync();
            _localStorageCache = new LocalStorageCacheServiceSync(_storageCache);
        }

        [Theory]
        [InlineData(1, "Test")]
        [InlineData(2, "Test2")]
        [InlineData(3, "Test3")]
        [InlineData(4, "Test4")]
        public void GetOrCreateCache_ValidKey_CacheShouldBeRetrievedFromCache(int id, string name)
        {
            //Arrange
            var cacheSaved = new DummyObject(id, name);
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            var cacheRetrieved = _localStorageCache.GetOrCreateCache(
                  Key,
                   TimeSpan.FromHours(1),
                  () =>
                  {
                      return new DummyObject(11, "Test11");
                  });

            //Assert
            Assert.Equal(cacheSaved.Id, cacheRetrieved.Id);
            Assert.Equal(cacheSaved.Name, cacheRetrieved.Name);
        }

        [Fact]
        public void GetOrCreateCache_CacheExpired_NewCacheShouldBeReturned()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.SetCache(Key, cacheSaved);
            var newCache = new DummyObject(2, "Test2");

            //Act
            var cacheRetrieved = _localStorageCache.GetOrCreateCache(
                  Key,
                  TimeSpan.FromHours(-1),
                  () =>
                  {
                      return newCache;
                  });

            //Assert
            Assert.Equal(newCache.Id, cacheRetrieved.Id);
            Assert.Equal(newCache.Name, cacheRetrieved.Name);
        }

        [Fact]
        public void GetOrCreateCache_FirstTimeCacheIsCreated_NewCacheShouldBeReturned()
        {
            //Arrange
            var newCache = new DummyObject(2, "Test2");

            //Act
            var cacheRetrieved = _localStorageCache.GetOrCreateCache(
                  Key,
                  TimeSpan.FromHours(1),
                  () =>
                  {
                      return newCache;
                  });

            //Assert
            Assert.Equal(newCache.Id, cacheRetrieved.Id);
            Assert.Equal(newCache.Name, cacheRetrieved.Name);
        }

        [Fact]
        public void GetOrCreateCache_EmptyKey_ErrorShouldBeThrown()
        {
            //Arrange
            var newCache = new DummyObject(2, "Test2");

            //Act
            var action = new Action(() => _localStorageCache.GetOrCreateCache(
                  string.Empty,
                  TimeSpan.FromHours(1),
                  async () =>
                  {
                      return newCache;
                  }));

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void GetOrCreateCache_NonExistingKey_CreateNewCache()
        {
            //Arrange
            string nonExsitingKey = "NonExsitingKey";
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.SetCache(Key, cacheSaved);
            var newCache = new DummyObject(2, "Test2");

            //Act
            var cacheRetrieved = _localStorageCache.GetOrCreateCache(
                   nonExsitingKey,
                   TimeSpan.FromHours(1),
                   () =>
                   {
                       return newCache;
                   });

            //Assert
            Assert.NotNull(cacheRetrieved);
            Assert.Equal(newCache.Id, cacheRetrieved.Id);
            Assert.Equal(newCache.Name, cacheRetrieved.Name);
        }
    }
}
