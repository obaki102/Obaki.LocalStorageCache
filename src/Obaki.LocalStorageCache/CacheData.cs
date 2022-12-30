namespace Obaki.LocalStorageCache
{
    public class CacheData<T>
    {
        private DateTime _createDateTime;

        public CacheData()
        {
            _createDateTime = DateTime.UtcNow;
        }

        public CacheData(T data)
        {
            Cache = data;
            _createDateTime = DateTime.UtcNow;
        }

        public T? Cache { get; init; }

        public DateTime Created { get => _createDateTime; set => _createDateTime = value; }
    }
}