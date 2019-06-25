namespace Wingman.Container
{
    using System;
    using System.Collections.Generic;

    /// <summary> Retriever for dependencies registered with a <see cref="IDependencyRegistrar"/>. </summary>
    public interface IDependencyRetriever
    {
        /// <summary> Requests an instance of the specified type from the container. </summary>
        TService GetInstance<TService>(string key = null);

        /// <summary> Gets all instances of the specified type from the container. </summary>
        IEnumerable<TService> GetAllInstances<TService>();

        /// <summary> Requests an instance for the specified service type. </summary>
        /// <returns> The instance, or null if a handler is not found. </returns>
        object GetInstance(Type service, string key = null);

        /// <summary> Requests all instances of a given service type. </summary>
        /// <returns> All the instances, or an empty enumerable if none are found. </returns>
        IEnumerable<object> GetAllInstances(Type service);

        /// <summary> Pushes dependencies into an existing instance based on interface properties with setters. </summary>
        void BuildUp(object instance);
    }
}