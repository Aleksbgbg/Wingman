namespace Wingman.Container.Entries
{
    using System.Collections.Generic;

    using Wingman.Container.Strategies;

    internal class ServiceEntryStore : IServiceEntryStore
    {
        private readonly Dictionary<ServiceEntry, List<IServiceLocationStrategy>> _handlers = new Dictionary<ServiceEntry, List<IServiceLocationStrategy>>();

        public bool HasHandler(ServiceEntry serviceEntry)
        {
            return _handlers.ContainsKey(serviceEntry);
        }

        public void InsertHandler(ServiceEntry serviceEntry, IServiceLocationStrategy serviceLocationStrategy)
        {
            if (HasHandler(serviceEntry))
            {
                _handlers[serviceEntry].Add(serviceLocationStrategy);
            }
            else
            {
                _handlers.Add(serviceEntry, new List<IServiceLocationStrategy>
                {
                    serviceLocationStrategy
                });
            }
        }

        public void RemoveHandler(ServiceEntry serviceEntry)
        {
            _handlers.Remove(serviceEntry);
        }
    }
}