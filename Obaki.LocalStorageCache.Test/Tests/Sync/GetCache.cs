using Obaki.LocalStorageCache.Test.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obaki.LocalStorageCache.Test
{
    public class GetCache
    {

        private readonly InMemoryStorageCacheSync _storageCache;
        private readonly LocalStorageCacheServiceSync _localStorageCache;
        public const string Key = "TestKey";

        public GetCache()
        {
            _storageCache = new InMemoryStorageCacheSync();
            _localStorageCache = new LocalStorageCacheServiceSync(_storageCache);
        }


        [Fact]
        public void GetCache_ValidKey_CacheShouldBeRetrieved()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            var cacheRetrieved = _localStorageCache.GetCache<DummyObject>(Key);

            //Assert
            Assert.Equal(cacheSaved.Id, cacheRetrieved.Id);
            Assert.Equal(cacheSaved.Name, cacheRetrieved.Name);
        }

        [Fact]
        public void GetCache_EmptyKey_ShouldThrowAnError()
        {
            //Arrange
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            var action = new Action(() => _localStorageCache.GetCache<DummyObject>(string.Empty));

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void GetCache_NonExistingKey_ShouldReturnNull()
        {
            //Arrange
            string nonExistingKey = "EmptyKey";
            var cacheSaved = new DummyObject(1, "Test");
            _localStorageCache.SetCache(Key, cacheSaved);

            //Act
            var cacheRetrieved = _localStorageCache.GetCache<DummyObject>(nonExistingKey);

            //Assert
            Assert.Null(cacheRetrieved);
        }
    }
}
