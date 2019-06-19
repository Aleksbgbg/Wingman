namespace Wingman.Container
{
    using System;

    /// <summary> Default implementation of <see cref="IDependencyRegistrar"/>. </summary>
    public class DependencyRegistrar : DependencyRegistrarBase
    {
        private readonly IServiceEntryStore _serviceEntryStore;

        private readonly ILocationStrategyFactory _locationStrategyFactory;

        internal DependencyRegistrar(IServiceEntryStore serviceEntryStore, ILocationStrategyFactory locationStrategyFactory)
        {
            _serviceEntryStore = serviceEntryStore;
            _locationStrategyFactory = locationStrategyFactory;
        }

        public override void RegisterInstance(Type service, object implementation, string key = null)
        {
            InsertHandler(service, key, _locationStrategyFactory.CreateInstance(implementation));
        }

        public override void RegisterSingleton(Type service, Type implementation, string key = null)
        {
            InsertHandler(service, key, _locationStrategyFactory.CreateSingleton(implementation));
        }

        public override void RegisterPerRequest(Type service, Type implementation, string key = null)
        {
            InsertHandler(service, key, _locationStrategyFactory.CreatePerRequest(implementation));
        }

        public override void RegisterHandler(Type service, Func<IDependencyRetriever, object> handler, string key = null)
        {
            InsertHandler(service, key, _locationStrategyFactory.CreateHandler(handler));
        }

        public override void UnregisterHandler(Type service, string key = null)
        {
            _serviceEntryStore.RemoveHandler(MakeServiceEntry(service, key));
        }

        public override bool HasHandler(Type service, string key = null)
        {
            return _serviceEntryStore.HasHandler(MakeServiceEntry(service, key));
        }

        private void InsertHandler(Type service, string key, IServiceLocationStrategy serviceLocationStrategy)
        {
            _serviceEntryStore.InsertHandler(MakeServiceEntry(service, key), serviceLocationStrategy);
        }

        private static ServiceEntry MakeServiceEntry(Type serviceType, string key)
        {
            return new ServiceEntry(serviceType, key);
        }
    }
}