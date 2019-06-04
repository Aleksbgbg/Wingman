﻿namespace Wingman.ServiceFactory.Strategies
{
    using System;

    internal interface IRetrievalStrategyStore
    {
        bool IsRegistered(Type interfaceType);

        void InsertFromRetriever(Type interfaceType);

        void InsertPerRequest(Type interfaceType, Type concreteType);

        IServiceRetrievalStrategy RetrieveMappingFor(Type interfaceType);
    }
}