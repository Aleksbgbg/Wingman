namespace Wingman.ServiceFactory.Strategies
{
    using System;

    internal interface IRetrievalStrategyStore
    {
        bool IsRegistered(Type interfaceType);

        void Insert(Type interfaceType, IServiceRetrievalStrategy serviceRetrievalStrategy);

        IServiceRetrievalStrategy RetrieveMappingFor(Type interfaceType);
    }
}