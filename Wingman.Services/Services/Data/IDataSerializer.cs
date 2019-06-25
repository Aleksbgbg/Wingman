namespace Wingman.Services.Data
{
    /// <summary> Serializes data to and from a string. </summary>
    public interface IDataSerializer
    {
        /// <summary> Computes the string representation of the data object. </summary>
        string Serialize(object data);

        /// <summary> Deserializes the <paramref name="data"/> string into an object of type <typeparamref name="T"/>. </summary>
        T Deserialize<T>(string data);
    }
}