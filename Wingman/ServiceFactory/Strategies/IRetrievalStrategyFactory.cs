namespace Wingman.ServiceFactory.Strategies
{
    using System;

    internal interface IRetrievalStrategyFactory
    {
        IServiceRetrievalStrategy FromRetriever(Type interfaceType);

        IServiceRetrievalStrategy PerRequest(Type interfaceType, Type concreteType);
    }
}