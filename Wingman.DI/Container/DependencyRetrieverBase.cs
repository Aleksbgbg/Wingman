namespace Wingman.Container
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary> Convenience base class for implementing <see cref="IDependencyRetriever"/>. </summary>
    public abstract class DependencyRetrieverBase : IDependencyRetriever
    {
        /// <inheritdoc/>
        public TService GetInstance<TService>(string key = null)
        {
            return (TService)GetInstance(typeof(TService), key);
        }

        /// <inheritdoc/>
        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return GetAllInstances(typeof(TService)).Cast<TService>();
        }

        /// <inheritdoc/>
        public abstract object GetInstance(Type service, string key = null);

        /// <inheritdoc/>
        public abstract IEnumerable<object> GetAllInstances(Type service);

        /// <inheritdoc/>
        public abstract void BuildUp(object instance);
    }
}