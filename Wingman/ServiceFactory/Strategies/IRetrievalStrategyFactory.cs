namespace Wingman.ServiceFactory.Strategies
{
    using System;

    internal interface IRetrievalStrategyFactory
    {
        IServiceRetrievalStrategy CreateFromRetriever(Type interfaceType);

        IServiceRetrievalStrategy CreatePerRequest(Type concreteType);
    }
}