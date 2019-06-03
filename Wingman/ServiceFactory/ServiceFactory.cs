namespace Wingman.ServiceFactory
{
    using System;

    using Wingman.Container;
    using Wingman.Utilities;

    /// <summary> Default implementation of <see cref="IServiceFactory"/>. </summary>
    public class ServiceFactory : IServiceFactory, IServiceFactoryRegistrar
    {
        private readonly IDependencyRegistrar _dependencyRegistrar;

        private readonly IServiceRetrievalStrategyStore _serviceRetrievalStrategyStore;

        internal ServiceFactory(IDependencyRegistrar dependencyRegistrar,
                                IServiceRetrievalStrategyStore serviceRetrievalStrategyStore)
        {
            _dependencyRegistrar = dependencyRegistrar;
            _serviceRetrievalStrategyStore = serviceRetrievalStrategyStore;
        }

        /// <inheritdoc/>
        public void RegisterFromRetriever<TService>()
        {
            RegisterFromRetriever(typeof(TService));
        }

        /// <inheritdoc/>
        public void RegisterPerRequest<TService, TImplementation>() where TImplementation : TService
        {
            RegisterPerRequest(typeof(TService), typeof(TImplementation));
        }

        /// <inheritdoc/>
        public TService Make<TService>(params object[] arguments)
        {
            return (TService)Make(typeof(TService), arguments);
        }

        private void RegisterFromRetriever(Type interfaceType)
        {
            EnsureNotPreviouslyRegistered(interfaceType);
            EnsureRetrieverHasHandler(interfaceType);

            _serviceRetrievalStrategyStore.InsertFromRetriever(interfaceType);
        }

        private void RegisterPerRequest(Type interfaceType, Type concreteType)
        {
            EnsureNotPreviouslyRegistered(interfaceType);
            EnsureIsConcrete(concreteType);

            _serviceRetrievalStrategyStore.InsertPerRequest(interfaceType, concreteType);
        }

        private void EnsureNotPreviouslyRegistered(Type interfaceType)
        {
            if (_serviceRetrievalStrategyStore.IsRegistered(interfaceType))
            {
                ThrowHelper.Throw.ServiceFactory.DuplicateRegistration(interfaceType);
            }
        }

        private void EnsureRetrieverHasHandler(Type interfaceType)
        {
            if (!_dependencyRegistrar.HasHandler(interfaceType))
            {
                ThrowHelper.Throw.ServiceFactory.NoHandlerRegisteredWithContainer(interfaceType);
            }
        }

        private void EnsureIsConcrete(Type concreteType)
        {
            if (concreteType.IsAbstract)
            {
                ThrowHelper.Throw.ServiceFactory.RegisterNonConcreteTypePerRequest(concreteType);
            }
        }

        private object Make(Type interfaceType, object[] arguments)
        {
            EnsureRegistered(interfaceType);

            IServiceRetrievalStrategy serviceRetrievalStrategy = _serviceRetrievalStrategyStore.RetrieveMappingFor(interfaceType);

            return serviceRetrievalStrategy.RetrieveService(arguments);
        }

        private void EnsureRegistered(Type interfaceType)
        {
            if (!_serviceRetrievalStrategyStore.IsRegistered(interfaceType))
            {
                ThrowHelper.Throw.ServiceFactory.NoDependencyMapping(interfaceType);
            }
        }
    }
}