namespace Wingman.ServiceFactory
{
    using System;

    internal interface IServiceRetrievalStrategyStore
    {
        bool IsRegistered(Type interfaceType);

        void InsertFromRetriever(Type interfaceType);

        void InsertPerRequest(Type interfaceType, Type concreteType);

        IServiceRetrievalStrategy RetrieveMappingFor(Type interfaceType);
    }
}