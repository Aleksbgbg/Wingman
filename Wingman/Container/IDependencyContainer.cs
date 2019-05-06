namespace Wingman.Container
{
    using System;
    using System.Collections.Generic;

    /// <summary> An IoC container. </summary>
    public interface IDependencyContainer
    {
        /// <summary> Occurs when a new dependency instance is created. </summary>
        event Action<object> Activated;

        /// <summary> Registers a singleton implementation of the service type. </summary>
        void RegisterInstance(Type service, string key, object implementation);

        /// <summary>
        ///     Registers the implementation type so that it is created once, on the first request,
        ///     and the same instance is returned for all requests thereafter.
        /// </summary>
        void RegisterSingleton(Type service, string key, Type implementation);

        /// <summary> Registers the implementation type so that a new instance is created on every request for the service type. </summary>
        void RegisterPerRequest(Type service, string key, Type implementation);

        /// <summary> Registers a custom handler function for serving requests for a service from the container. </summary>
        void RegisterHandler(Type service, string key, Func<IDependencyContainer, object> handler);

        /// <summary> Unregisters any handlers for the service/key that have previously been registered. </summary>
        void UnregisterHandler(Type service, string key);

        /// <summary> Determines if a handler for the service/key has previously been registered. </summary>
        /// <returns> True if a handler is registered; false otherwise. </returns>
        bool HasHandler(Type service, string key);

        /// <summary> Requests an instance for the specified service type. </summary>
        /// <returns> The instance, or null if a handler is not found. </returns>
        object GetInstance(Type service, string key);

        /// <summary> Requests all instances of a given service type. </summary>
        /// <returns> All the instances, or an empty enumerable if none are found. </returns>
        IEnumerable<object> GetAllInstances(Type service);

        /// <summary> Pushes dependencies into an existing instance based on interface properties with setters. </summary>
        void BuildUp(object instance);
    }
}