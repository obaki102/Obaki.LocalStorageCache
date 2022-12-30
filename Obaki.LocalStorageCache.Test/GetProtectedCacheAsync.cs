using Obaki.LocalStorageCache.Test.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obaki.LocalStorageCache.Test
{
    public class GetProtectedCacheAsync
    {

        private readonly InMemoryStorageCache _storageCache;
        private readonly LocalStorageCacheProvider _localStorageCache;
        private readonly TestDataProtectionProvider _testDataProtectionProvider;
        public const string Key = "TestKey";

        public GetProtectedCacheAsync()
        {
            _storageCache = new InMemoryStorageCache();
            _testDataProtectionProvider = new TestDataProtectionProvider();
            _localStorageCache = new LocalStorageCacheProvider(_storageCache, _testDataProtectionProvider);
        }
       
        [Fact]
        public async Task GetProtectedCacheAsync_ValidKey_DataShouldBeRetrieved()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheSaved);

            //Act
            var cacheRetrieved = await _localStorageCache.GetProtectedCacheAsync<DummyObject>(Key);

            //Assert
            Assert.Equal(cacheSaved.Id, cacheRetrieved.Id);
            Assert.Equal(cacheSaved.Name, cacheRetrieved.Name);
        }

        [Fact]
        public async Task GetProtectedCacheAsync_EmptyKey_ShouldThrowAnError()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheSaved);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.GetProtectedCacheAsync<DummyObject>(string.Empty));

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task GetProtectedCacheAsync_NonExistingKey_ShouldReturnAnException()
        {
            //Arrange
            string nonExistingKey = "nonExistingKey";
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheSaved);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.GetProtectedCacheAsync<DummyObject>(nonExistingKey));

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(function);
        }

        [Fact]
        public async Task GetProtectedCacheAsync_CacheIsUnprotected_ShouldThrowAnError()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetCacheAsync(Key, cacheSaved);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.GetProtectedCacheAsync<DummyObject>(Key));

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(function);
        }
    }
}
