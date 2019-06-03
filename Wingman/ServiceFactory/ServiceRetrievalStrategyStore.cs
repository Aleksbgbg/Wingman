namespace Wingman.ServiceFactory
{
    using System;
    using System.Collections.Generic;

    internal class ServiceRetrievalStrategyStore : IServiceRetrievalStrategyStore
    {
        private readonly Dictionary<Type, IServiceRetrievalStrategy> _strategies = new Dictionary<Type, IServiceRetrievalStrategy>();

        public bool IsRegistered(Type interfaceType)
        {
            return _strategies.ContainsKey(interfaceType);
        }

        public void InsertFromRetriever(Type interfaceType)
        {
            _strategies[interfaceType] = new FromRetrieverRetrievalStrategy();
        }

        public void InsertPerRequest(Type interfaceType, Type concreteType)
        {
            _strategies[interfaceType] = new PerRequestRetrievalStrategy();
        }

        public IServiceRetrievalStrategy RetrieveMappingFor(Type interfaceType)
        {
            return _strategies[interfaceType];
        }
    }
}