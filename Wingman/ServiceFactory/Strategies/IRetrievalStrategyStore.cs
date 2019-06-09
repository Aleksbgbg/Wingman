namespace Wingman.ServiceFactory.Strategies
{
    using System;

    internal interface IRetrievalStrategyStore
    {
        bool IsRegistered(Type interfaceType);

        void Insert(Type interfaceType, IServiceRetrievalStrategy serviceRetrievalStrategy);

        void InsertFromRetriever(Type interfaceType);

        void InsertPerRequest(Type interfaceType, Type concreteType);

        IServiceRetrievalStrategy RetrieveMappingFor(Type interfaceType);
    }
}