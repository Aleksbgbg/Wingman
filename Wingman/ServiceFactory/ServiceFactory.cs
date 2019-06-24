namespace Wingman.ServiceFactory
{
    using System;

    using Wingman.ServiceFactory.Strategies;
    using Wingman.Utilities.ThrowHelper;

    /// <summary> Default implementation of <see cref="IServiceFactory"/>. </summary>
    public class ServiceFactory : IServiceFactory
    {
        private readonly IRetrievalStrategyStore _retrievalStrategyStore;

        internal ServiceFactory(IRetrievalStrategyStore retrievalStrategyStore)
        {
            _retrievalStrategyStore = retrievalStrategyStore;
        }

        /// <inheritdoc/>
        public TService Create<TService>(params object[] arguments)
        {
            return (TService)Create(typeof(TService), arguments);
        }

        private object Create(Type interfaceType, object[] arguments)
        {
            EnsureRegistered(interfaceType);

            IServiceRetrievalStrategy serviceRetrievalStrategy = _retrievalStrategyStore.RetrieveMappingFor(interfaceType);

            return serviceRetrievalStrategy.RetrieveService(arguments);
        }

        private void EnsureRegistered(Type interfaceType)
        {
            if (!_retrievalStrategyStore.IsRegistered(interfaceType))
            {
                throw ThrowHelper.ServiceFactory.NoDependencyMapping(interfaceType);
            }
        }
    }
}