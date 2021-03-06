﻿namespace Wingman.ServiceFactory.Strategies
{
    using System;

    using Wingman.Container;
    using Wingman.Utilities.ThrowHelper;

    internal class FromRetrieverRetrievalStrategy : IServiceRetrievalStrategy
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        private readonly Type _interfaceType;

        public FromRetrieverRetrievalStrategy(IDependencyRetriever dependencyRetriever, Type interfaceType)
        {
            _dependencyRetriever = dependencyRetriever;
            _interfaceType = interfaceType;
        }

        public object RetrieveService(object[] arguments)
        {
            EnsureNoArgumentsPassed(arguments);

            return _dependencyRetriever.GetInstance(_interfaceType);
        }

        private void EnsureNoArgumentsPassed(object[] arguments)
        {
            if (arguments.Length != 0)
            {
                throw ThrowHelper.FromRetrieverRetrievalStrategy.ArgumentsNotEmpty(nameof(arguments), arguments);
            }
        }
    }
}