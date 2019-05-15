namespace Wingman.Container
{
    using System;

    using Caliburn.Micro;

    /// <summary> An implementation adapter for the default Caliburn.Micro IoC container. </summary>
    public class DependencyContainer : SimpleContainer, IDependencyActivator, IDependencyRegistrar, IDependencyRetriever
    {
        /// <inheritdoc/>
        public void RegisterHandler(Type service, string key, Func<IDependencyRetriever, object> handler)
        {
            RegisterHandler(service, key, (SimpleContainer container) => handler((IDependencyRetriever)container));
        }
    }
}