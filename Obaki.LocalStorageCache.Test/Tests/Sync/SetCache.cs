using Obaki.LocalStorageCache.Test.TestHelper;

namespace Obaki.LocalStorageCache.Test
{
    public class SetCache
    {
        private readonly InMemoryStorageCacheSync _storageCache;
        private readonly LocalStorageCacheServiceSync _localStorageCache;
        public const string Key = "TestKey";

        public SetCache()
        {
            _storageCache = new InMemoryStorageCacheSync();
            _localStorageCache = new LocalStorageCacheServiceSync(_storageCache);
        }


        [Fact]
        public void SetCache_ValidValueEntered_CacheShouldBeSaved()
        {
            //Arrange
            var cacheToSave = new DummyObject(1, "Test");

            //Act
            _localStorageCache.SetCache(Key, cacheToSave);

            //Assert
            var cacheFromStorage = _localStorageCache.GetCache<DummyObject>(Key);
            Assert.Equal(cacheToSave.Id, cacheFromStorage.Id);
            Assert.Equal(cacheToSave.Name, cacheFromStorage.Name);
        }

        [Fact]
        public void SetCache_ExistingValueWithSameKey_CacheShouldBeOverwritten()
        {
            //Arrange
            var cacheToSave = new DummyObject(1, "Test");
            var cacheToSave2 = new DummyObject(2, "Test2");

            //Act
            _localStorageCache.SetCache(Key, cacheToSave);
            _localStorageCache.SetCache(Key, cacheToSave2);

            //Assert
            var cacheFromStorage = _localStorageCache.GetCache<DummyObject>(Key);
            Assert.Equal(cacheToSave2.Id, cacheFromStorage.Id);
            Assert.Equal(cacheToSave2.Name, cacheFromStorage.Name);
        }

        [Fact]
        public void SetCache_EmptyKey_ShouldThrowAnError()
        {
            //Arrange
            var cacheToSave = new DummyObject(1, "Test");

            //Act
            var action = new Action(() => _localStorageCache.SetCache(string.Empty, cacheToSave));

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetCache_EmptyCache_ShouldThrowAnError()
        {
            //Arrange
            var cacheToSave = default(DummyObject);

            //Act
            var action = new Action(() => _localStorageCache.SetCache(Key, cacheToSave));

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetCache_EmptyKeyAndCache_ShouldThrowAnError()
        {
            //Arrange
            var cacheToSave = default(DummyObject);

            //Act
            var action = new Action(() => _localStorageCache.SetCache(string.Empty, cacheToSave));

            //Assert
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
