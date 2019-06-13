namespace Wingman.Services.Data
{
    using System;

    /// <summary> Default implementation of <see cref="IDataService"/>. </summary>
    public class DataService : IDataService
    {
        private readonly IDataSerializer _dataSerializer;

        private readonly IPersistentStore _persistentStore;

        public DataService(IDataSerializer dataSerializer, IPersistentStore persistentStore)
        {
            _dataSerializer = dataSerializer;
            _persistentStore = persistentStore;
        }

        /// <inheritdoc/>
        public void Save(string dataName, object data)
        {
            _persistentStore.Save(dataName, _dataSerializer.Serialize(data));
        }

        /// <inheritdoc/>
        public T Load<T>(string dataName, T emptyData = default)
        {
            if (_persistentStore.Contains(dataName))
            {
                return LoadAndDeserialize<T>(dataName);
            }

            return emptyData;
        }

        /// <inheritdoc/>
        public T Load<T>(string dataName, Func<T> emptyData)
        {
            if (_persistentStore.Contains(dataName))
            {
                return LoadAndDeserialize<T>(dataName);
            }

            return emptyData();
        }

        private T LoadAndDeserialize<T>(string dataName)
        {
            return _dataSerializer.Deserialize<T>(_persistentStore.Load(dataName));
        }
    }
}