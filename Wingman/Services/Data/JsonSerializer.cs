namespace Wingman.Services.Data
{
    using Newtonsoft.Json;

    /// <summary> Default implementation of <see cref="IDataSerializer"/>. Converts to JSON via Newtonsoft.Json. </summary>
    public class JsonSerializer : IDataSerializer
    {
        /// <inheritdoc/>
        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <inheritdoc/>
        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}