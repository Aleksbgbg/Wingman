namespace Wingman.Services.Data
{
    /// <summary> Stores data in a persistent format, defined by the implementation. </summary>
    public interface IPersistentStore
    {
        /// <summary> Checks whether the <paramref name="dataName"/> has previously been saved in the store. </summary>
        bool Contains(string dataName);

        /// <summary> Saves the <paramref name="data"/> string under the <paramref name="dataName"/> in the store. </summary>
        void Save(string dataName, string data);

        /// <summary> Loads the data string stored under the <paramref name="dataName"/> in the store. </summary>
        string Load(string dataName);
    }
}