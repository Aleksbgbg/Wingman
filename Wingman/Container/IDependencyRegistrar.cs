namespace Wingman.Container
{
    using System;

    /// <summary> Registrar for dependencies that are retrieved via a <see cref="IDependencyRetriever"/>. </summary>
    public interface IDependencyRegistrar
    {
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
        void RegisterHandler(Type service, string key, Func<IDependencyRetriever, object> handler);

        /// <summary> Unregisters any handlers for the service/key that have previously been registered. </summary>
        void UnregisterHandler(Type service, string key);

        /// <summary> Determines if a handler for the service/key has previously been registered. </summary>
        /// <returns> True if a handler is registered; false otherwise. </returns>
        bool HasHandler(Type service, string key);
    }
}