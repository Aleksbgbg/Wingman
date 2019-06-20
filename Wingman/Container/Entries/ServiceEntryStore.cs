namespace Wingman.Container.Entries
{
    using System.Collections.Generic;

    using Wingman.Container.Strategies;

    internal class ServiceEntryStore : IServiceEntryStore
    {
        private readonly Dictionary<ServiceEntry, IServiceLocationStrategy> _handlers = new Dictionary<ServiceEntry, IServiceLocationStrategy>();

        public bool HasHandler(ServiceEntry serviceEntry)
        {
            return _handlers.ContainsKey(serviceEntry);
        }

        public void InsertHandler(ServiceEntry serviceEntry, IServiceLocationStrategy serviceLocationStrategy)
        {
            _handlers.Add(serviceEntry, serviceLocationStrategy);
        }

        public void RemoveHandler(ServiceEntry serviceEntry)
        {
            _handlers.Remove(serviceEntry);
        }
    }
}