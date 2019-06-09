namespace Wingman.ServiceFactory.Strategies
{
    using System;
    using System.Collections.Generic;

    internal class RetrievalStrategyStore : IRetrievalStrategyStore
    {
        private readonly IRetrievalStrategyFactory _retrievalStrategyFactory;

        private readonly Dictionary<Type, IServiceRetrievalStrategy> _strategies = new Dictionary<Type, IServiceRetrievalStrategy>();

        public RetrievalStrategyStore(IRetrievalStrategyFactory retrievalStrategyFactory)
        {
            _retrievalStrategyFactory = retrievalStrategyFactory;
        }

        public bool IsRegistered(Type interfaceType)
        {
            return _strategies.ContainsKey(interfaceType);
        }

        public void Insert(Type interfaceType, IServiceRetrievalStrategy serviceRetrievalStrategy)
        {
            throw new NotImplementedException();
        }

        public void InsertFromRetriever(Type interfaceType)
        {
            _strategies[interfaceType] = _retrievalStrategyFactory.FromRetriever(interfaceType);
        }

        public void InsertPerRequest(Type interfaceType, Type concreteType)
        {
            _strategies[interfaceType] = _retrievalStrategyFactory.PerRequest(concreteType);
        }

        public IServiceRetrievalStrategy RetrieveMappingFor(Type interfaceType)
        {
            return _strategies[interfaceType];
        }
    }
}