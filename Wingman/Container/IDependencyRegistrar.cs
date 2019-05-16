namespace Wingman.Container
{
    using System;
    using System.Reflection;

    /// <summary> Registrar for dependencies that are retrieved via a <see cref="IDependencyRetriever"/>. </summary>
    public interface IDependencyRegistrar
    {
        /// <summary> Registers a singleton instance. </summary>
        void Instance<TService>(TService instance, string key = null);

        /// <summary> Registers a singleton implementation type. </summary>
        void Singleton<TImplementation>(string key = null);

        /// <summary> Registers a singleton service type with the implementation type. </summary>
        void Singleton<TService, TImplementation>(string key = null) where TImplementation : TService;

        /// <summary> Registers a service implementation type to be instantiated on each request. </summary>
        void PerRequest<TImplementation>(string key = null);

        /// <summary> Registers a service to be instantiated on each request. </summary>
        void PerRequest<TService, TImplementation>(string key = null) where TImplementation : TService;

        /// <summary> Registers a custom service handler to be executed on each request. </summary>
        void Handler<TService>(Func<IDependencyRetriever, object> handler, string key = null);

        /// <summary> Registers all matching concrete types of <typeparamref name="TService"/> in an assembly as singletons. </summary>
        void RegisterAllTypesOf<TService>(Assembly assembly, Func<Type, bool> matchFilter = null, string key = null);

        /// <summary> Unregisters any handlers for the service/key that have previously been registered. </summary>
        void Unregister<TService>(string key = null);

        /// <summary> Determines if a handler for the service/key has previously been registered. </summary>
        bool HasHandler<TService>(string key = null);

        /// <summary> Registers a singleton implementation of the service type. </summary>
        void RegisterInstance(Type service, object implementation, string key = null);

        /// <summary>
        ///     Registers the implementation type so that it is created once, on the first request,
        ///     and the same instance is returned for all requests thereafter.
        /// </summary>
        void RegisterSingleton(Type service, Type implementation, string key = null);

        /// <summary> Registers the implementation type so that a new instance is created on every request for the service type. </summary>
        void RegisterPerRequest(Type service, Type implementation, string key = null);

        /// <summary> Registers a custom handler function for serving requests for a service from the container. </summary>
        void RegisterHandler(Type service, Func<IDependencyRetriever, object> handler, string key = null);

        /// <summary> Unregisters any handlers for the service/key that have previously been registered. </summary>
        void UnregisterHandler(Type service, string key = null);

        /// <summary> Determines if a handler for the service/key has previously been registered. </summary>
        /// <returns> True if a handler is registered; false otherwise. </returns>
        bool HasHandler(Type service, string key = null);
    }
}