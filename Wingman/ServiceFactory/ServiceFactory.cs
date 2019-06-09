namespace Wingman.ServiceFactory
{
    using System;

    using Wingman.ServiceFactory.Strategies;
    using Wingman.Utilities;

    /// <summary> Default implementation of <see cref="IServiceFactory"/>. </summary>
    public class ServiceFactory : IServiceFactory
    {
        private readonly IRetrievalStrategyStore _retrievalStrategyStore;

        internal ServiceFactory(IRetrievalStrategyStore retrievalStrategyStore)
        {
            _retrievalStrategyStore = retrievalStrategyStore;
        }

        /// <inheritdoc/>
        public TService Make<TService>(params object[] arguments)
        {
            return (TService)Make(typeof(TService), arguments);
        }

        private object Make(Type interfaceType, object[] arguments)
        {
            EnsureRegistered(interfaceType);

            IServiceRetrievalStrategy serviceRetrievalStrategy = _retrievalStrategyStore.RetrieveMappingFor(interfaceType);

            return serviceRetrievalStrategy.RetrieveService(arguments);
        }

        private void EnsureRegistered(Type interfaceType)
        {
            if (!_retrievalStrategyStore.IsRegistered(interfaceType))
            {
                ThrowHelper.Throw.ServiceFactory.NoDependencyMapping(interfaceType);
            }
        }
    }
}