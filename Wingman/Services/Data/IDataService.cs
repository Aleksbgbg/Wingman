namespace Wingman.Services.Data
{
    using System;

    /// <summary> Service which saves and loads data to a persistent store. </summary>
    public interface IDataService
    {
        /// <summary> Serializes the data object and saves to persistent store by name. </summary>
        void Save(string dataName, object data);

        /// <summary> Loads data by name from the persistent store, returning the <paramref name="emptyData"/> object if the <paramref name="dataName"/> does not exist. </summary>
        T Load<T>(string dataName, T emptyData = default);

        /// <summary>
        /// Loads data by name from the persistent store, returning the evaluated <paramref name="emptyData"/> object if the <paramref name="dataName"/> does not exist.
        /// Use when the <paramref name="emptyData"/> object is more expensive to compute than a function expression is.
        /// </summary>
        T Load<T>(string dataName, Func<T> emptyData);
    }
}