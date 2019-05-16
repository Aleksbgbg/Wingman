namespace Wingman.Container
{
    using System;

    using Caliburn.Micro;

    /// <summary> An implementation adapter for the default Caliburn.Micro IoC container. </summary>
    public class DependencyContainer : SimpleContainer, IDependencyActivator, IDependencyRegistrar, IDependencyRetriever
    {
        /// <inheritdoc/>
        public void RegisterInstance(Type service, object implementation, string key = null)
        {
            RegisterInstance(service, key, implementation);
        }

        /// <inheritdoc/>
        public void RegisterSingleton(Type service, Type implementation, string key = null)
        {
            RegisterSingleton(service, key, implementation);
        }

        /// <inheritdoc/>
        public void RegisterPerRequest(Type service, Type implementation, string key = null)
        {
            RegisterPerRequest(service, key, implementation);
        }

        /// <inheritdoc/>
        public void RegisterHandler(Type service, Func<IDependencyRetriever, object> handler, string key = null)
        {
            RegisterHandler(service, key, container => handler((IDependencyRetriever)container));
        }
    }
}