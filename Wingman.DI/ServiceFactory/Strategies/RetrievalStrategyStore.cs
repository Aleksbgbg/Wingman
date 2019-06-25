namespace Wingman.ServiceFactory.Strategies
{
    using System;
    using System.Collections.Generic;

    internal class RetrievalStrategyStore : IRetrievalStrategyStore
    {
        private readonly Dictionary<Type, IServiceRetrievalStrategy> _strategies = new Dictionary<Type, IServiceRetrievalStrategy>();

        public bool IsRegistered(Type interfaceType)
        {
            return _strategies.ContainsKey(interfaceType);
        }

        public void Insert(Type interfaceType, IServiceRetrievalStrategy serviceRetrievalStrategy)
        {
            _strategies.Add(interfaceType, serviceRetrievalStrategy);
        }

        public IServiceRetrievalStrategy RetrieveMappingFor(Type interfaceType)
        {
            return _strategies[interfaceType];
        }
    }
}