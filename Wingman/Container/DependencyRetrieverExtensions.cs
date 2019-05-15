namespace Wingman.Container
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary> Convenience extension methods for all implementations of <see cref="IDependencyRetriever"/>. </summary>
    public static class DependencyRetrieverExtensions
    {
        /// <summary> Requests an instance of the specified type from the container. </summary>
        public static TService GetInstance<TService>(this IDependencyRetriever container, string key = null)
        {
            return (TService)container.GetInstance(typeof(TService), key);
        }

        /// <summary> Gets all instances of the specified type from the container. </summary>
        public static IEnumerable<TService> GetAllInstances<TService>(this IDependencyRetriever container)
        {
            return container.GetAllInstances(typeof(TService)).Cast<TService>();
        }
    }
}