using Obaki.LocalStorageCache.Test.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obaki.LocalStorageCache.Test
{
    public class TryGetProtectedCacheAsync
    {
        private readonly InMemoryStorageCache _storageCache;
        private readonly LocalStorageCacheProvider _localStorageCache;
        private readonly TestDataProtectionProvider _testDataProtectionProvider;
        public const string Key = "TestKey";

        public TryGetProtectedCacheAsync()
        {
            _storageCache = new InMemoryStorageCache();
            _testDataProtectionProvider = new TestDataProtectionProvider();
            _localStorageCache = new LocalStorageCacheProvider(_storageCache, _testDataProtectionProvider);
        }

        [Fact]
        public async Task TryGetProtectedCacheAsync_ValidValueEntered_CacheDataShouldBeReturned()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.CacheExpiration = TimeSpan.FromHours(1);
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheSaved);

            //Act
            var (isCacheExist, cacheData) = await _localStorageCache.TryGetProtectedCacheAsync<DummyObject>(Key);

            //Assert
            Assert.True(isCacheExist);
            Assert.Equal(cacheSaved.Id, cacheData.Id);
            Assert.Equal(cacheSaved.Name, cacheData.Name);
        }

        [Fact]
        public async Task TryGetProtectedCacheAsync_ExpiredCache_CacheDataShouldReturnFalseAndCacheDataIsNull()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.CacheExpiration = TimeSpan.FromHours(-1);
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheSaved);

            //Act
            var (isCacheExist, cacheData) = await _localStorageCache.TryGetProtectedCacheAsync<DummyObject>(Key);

            //Assert
            Assert.False(isCacheExist);
            Assert.Null(cacheData);

        }

        [Fact]
        public void TryGetProtectedCacheAsync_EmptyKey_ShouldThrowAnError()
        {
            //Act
            var function = new Func<Task>(async () => await _localStorageCache.TryGetProtectedCacheAsync<DummyObject>(string.Empty));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public async Task TryGetProtectedCacheAsync_NonExistingKey_ShouldReturnAnException()
        {
            //Arrange
            string nonExistingKey = "EmptyKey";
            var cacheSaved = new DummyObject(1, "Test");
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheSaved);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.GetProtectedCacheAsync<DummyObject>(nonExistingKey));

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(function);
        }
    }
}
