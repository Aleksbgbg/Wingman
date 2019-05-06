namespace Wingman.Container
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary> Convenience extension methods for all implementations of <see cref="IDependencyContainer"/>. </summary>
    public static class DependencyContainerExtensions
    {
        /// <summary> Registers a singleton instance. </summary>
        public static void Instance<TService>(this IDependencyContainer container, TService instance, string key = null)
        {
            container.RegisterInstance(typeof(TService), key, instance);
        }

        /// <summary> Registers a singleton implementation type. </summary>
        public static void Singleton<TImplementation>(this IDependencyContainer container, string key = null)
        {
            container.Singleton<TImplementation, TImplementation>(key);
        }

        /// <summary> Registers a singleton service type with the implementation type. </summary>
        public static void Singleton<TService, TImplementation>(this IDependencyContainer container, string key = null)
                where TImplementation : TService
        {
            container.RegisterSingleton(typeof(TService), key, typeof(TImplementation));
        }

        /// <summary> Registers a service implementation type to be instantiated on each request. </summary>
        public static void PerRequest<TImplementation>(this IDependencyContainer container, string key = null)
        {
            container.RegisterPerRequest(typeof(TImplementation), key, typeof(TImplementation));
        }

        /// <summary> Registers a service to be instantiated on each request. </summary>
        public static void PerRequest<TService, TImplementation>(this IDependencyContainer container, string key = null)
                where TImplementation : TService
        {
            container.RegisterPerRequest(typeof(TService), key, typeof(TImplementation));
        }

        /// <summary> Registers a custom service handler to be executed on each request. </summary>
        public static void Handler<TService>(this IDependencyContainer container, Func<IDependencyContainer, object> handler, string key = null)
        {
            container.RegisterHandler(typeof(TService), key, handler);
        }

        /// <summary> Registers all matching concrete types of <typeparamref name="TService"/> in an assembly as singletons. </summary>
        public static void RegisterAllTypesOf<TService>(this IDependencyContainer container, Assembly assembly, Func<Type, bool> matchFilter = null, string key = null)
        {
            Type serviceType = typeof(TService);

            bool IsMatchNoFilter(Type type)
            {
                return serviceType.IsAssignableFrom(type) && !type.IsAbstract;
            }

            bool IsMatchWithFilter(Type type)
            {
                return IsMatchNoFilter(type) && matchFilter(type);
            }

            Type[] assemblyTypes = assembly.GetTypes();
            IEnumerable<Type> matchingAssemblyTypes = matchFilter == null ? assemblyTypes.Where(IsMatchNoFilter) : assemblyTypes.Where(IsMatchWithFilter);

            foreach (Type type in matchingAssemblyTypes)
            {
                container.RegisterSingleton(typeof(TService), key, type);
            }
        }

        /// <summary> Unregisters any handlers for the service/key that have previously been registered. </summary>
        public static void Unregister<TService>(this IDependencyContainer container, string key = null)
        {
            container.UnregisterHandler(typeof(TService), key);
        }

        /// <summary> Determines if a handler for the service/key has previously been registered. </summary>
        public static bool HasHandler<TService>(this IDependencyContainer container, string key = null)
        {
            return container.HasHandler(typeof(TService), key);
        }

        /// <summary> Requests an instance of the specified type from the container. </summary>
        public static TService GetInstance<TService>(this IDependencyContainer container, string key = null)
        {
            return (TService)container.GetInstance(typeof(TService), key);
        }

        /// <summary> Gets all instances of the specified type from the container. </summary>
        public static IEnumerable<TService> GetAllInstances<TService>(this IDependencyContainer container)
        {
            return container.GetAllInstances(typeof(TService))
                            .Cast<TService>();
        }
    }
}