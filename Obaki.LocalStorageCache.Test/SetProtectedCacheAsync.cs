using Microsoft.AspNetCore.DataProtection;
using Moq;
using Obaki.LocalStorageCache.Test.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obaki.LocalStorageCache.Test
{
    public class SetProtectedCacheAsync
    {
        private readonly InMemoryStorageCache _storageCache;
        private readonly LocalStorageCacheProvider _localStorageCache;
        private readonly TestDataProtectionProvider _testDataProtectionProvider;
        public const string Key = "TestKey";

        public SetProtectedCacheAsync()
        {
            _storageCache = new InMemoryStorageCache();
            _testDataProtectionProvider = new TestDataProtectionProvider();
             _localStorageCache = new LocalStorageCacheProvider(_storageCache, _testDataProtectionProvider);
        }

        [Fact]
        public async Task SetProtectedCacheAsync_ValidValueEntered_DataShouldBeSaved()
        {
            //Arrange
            var cacheToSave = new DummyObject(1, "Test");

            //Act
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheToSave);

            //Assert
            var cacheFromStorage = await _localStorageCache.GetProtectedCacheAsync<DummyObject>(Key);
            Assert.Equal(cacheToSave.Id, cacheFromStorage.Id);
            Assert.Equal(cacheToSave.Name, cacheFromStorage.Name);
        }

        [Fact]
        public async Task SetProtectedCacheAsync_ExistingValueWithSameKey_DataShouldBeOverwritten()
        {
            //Arrange
            var cacheToSave = new DummyObject(1, "Test");
            var cacheToSave2 = new DummyObject(2, "Test2");

            //Act
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheToSave);
            await _localStorageCache.SetProtectedCacheAsync(Key, cacheToSave2);

            //Assert
            var cacheFromStorage = _localStorageCache.GetProtectedCacheAsync<DummyObject>(Key).Result;
            Assert.Equal(cacheToSave2.Id, cacheFromStorage.Id);
            Assert.Equal(cacheToSave2.Name, cacheFromStorage.Name);
        }

        [Fact]
        public void SetProtectedCacheAsync_EmptyKey_ShouldThrowAnError()
        {
            //Arrange
            var cacheToSave = new DummyObject(1, "Test");

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.SetProtectedCacheAsync(string.Empty, cacheToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public void SetProtectedCacheAsync_EmptyData_ShouldThrowAnError()
        {
            //Arrange
            var cacheToSave = default(DummyObject);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.SetProtectedCacheAsync(Key, cacheToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }

        [Fact]
        public void SetProtectedCacheAsync_EmptyKeyAndData_ShouldThrowAnError()
        {
            //Arrange
            var cacheToSave = default(DummyObject);

            //Act
            var function = new Func<Task>(async () => await _localStorageCache.SetProtectedCacheAsync(string.Empty, cacheToSave));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(function);
        }
    }
}
