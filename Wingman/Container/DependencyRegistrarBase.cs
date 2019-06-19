namespace Wingman.Container
{
    using System;
    using System.Reflection;

    /// <summary> Convenience base class for implementing <see cref="IDependencyRegistrar"/>. </summary>
    public abstract class DependencyRegistrarBase : IDependencyRegistrar
    {
        /// <inheritdoc/>
        public void Instance<TService>(TService instance, string key = null)
        {
            RegisterInstance(typeof(TService), instance, key);
        }

        /// <inheritdoc/>
        public void Singleton<TImplementation>(string key = null)
        {
            Singleton<TImplementation, TImplementation>(key);
        }

        /// <inheritdoc/>
        public void Singleton<TService, TImplementation>(string key = null) where TImplementation : TService
        {
            RegisterSingleton(typeof(TService), typeof(TImplementation), key);
        }

        /// <inheritdoc/>
        public void PerRequest<TImplementation>(string key = null)
        {
            RegisterPerRequest(typeof(TImplementation), typeof(TImplementation), key);
        }

        /// <inheritdoc/>
        public void PerRequest<TService, TImplementation>(string key = null) where TImplementation : TService
        {
            RegisterPerRequest(typeof(TService), typeof(TImplementation), key);
        }

        /// <inheritdoc/>
        public void Handler<TService>(Func<IDependencyRetriever, object> handler, string key = null)
        {
            RegisterHandler(typeof(TService), handler, key);
        }

        /// <inheritdoc/>
        public void RegisterAllTypesOf<TService>(Assembly assembly, Func<Type, bool> matchFilter = null, string key = null)
        {
            DependencyRegistrarExtensions.RegisterAllTypesOf<TService>(this, assembly, matchFilter, key);
        }

        /// <inheritdoc/>
        public void Unregister<TService>(string key = null)
        {
            UnregisterHandler(typeof(TService), key);
        }

        /// <inheritdoc/>
        public bool HasHandler<TService>(string key = null)
        {
            return HasHandler(typeof(TService), key);
        }

        /// <inheritdoc/>
        public abstract void RegisterInstance(Type service, object implementation, string key = null);

        /// <inheritdoc/>
        public abstract void RegisterSingleton(Type service, Type implementation, string key = null);

        /// <inheritdoc/>
        public abstract void RegisterPerRequest(Type service, Type implementation, string key = null);

        /// <inheritdoc/>
        public abstract void RegisterHandler(Type service, Func<IDependencyRetriever, object> handler, string key = null);

        /// <inheritdoc/>
        public abstract void UnregisterHandler(Type service, string key = null);

        /// <inheritdoc/>
        public abstract bool HasHandler(Type service, string key = null);
    }
}