namespace Wingman.Container
{
    using System;

    using Caliburn.Micro;

    /// <summary> An implementation adapter for the default Caliburn.Micro IoC container. Implements <see cref="IDependencyContainer"/>. </summary>
    public class DependencyContainer : SimpleContainer, IDependencyContainer
    {
        /// <inheritdoc cref="SimpleContainer.RegisterHandler"/>
        public void RegisterHandler(Type service, string key, Func<IDependencyContainer, object> handler)
        {
            RegisterHandler(service, key, (SimpleContainer container) => handler((IDependencyContainer)container));
        }
    }
}