namespace Wingman.ServiceFactory
{
    using System;

    using Wingman.Container;
    using Wingman.ServiceFactory.Strategies;
    using Wingman.Utilities.ThrowHelper;

    /// <summary> Default implementation of <see cref="IServiceFactoryRegistrar"/>. </summary>
    public class ServiceFactoryRegistrar : IServiceFactoryRegistrar
    {
        private readonly IDependencyRegistrar _dependencyRegistrar;

        private readonly IRetrievalStrategyFactory _retrievalStrategyFactory;

        private readonly IRetrievalStrategyStore _retrievalStrategyStore;

        internal ServiceFactoryRegistrar(IDependencyRegistrar dependencyRegistrar,
                                         IRetrievalStrategyFactory retrievalStrategyFactory,
                                         IRetrievalStrategyStore retrievalStrategyStore)
        {
            _dependencyRegistrar = dependencyRegistrar;
            _retrievalStrategyFactory = retrievalStrategyFactory;
            _retrievalStrategyStore = retrievalStrategyStore;
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

        private void RegisterFromRetriever(Type interfaceType)
        {
            EnsureNotPreviouslyRegistered(interfaceType);
            EnsureRetrieverHasHandler(interfaceType);

            _retrievalStrategyStore.Insert(interfaceType, _retrievalStrategyFactory.CreateFromRetriever(interfaceType));
        }

        private void RegisterPerRequest(Type interfaceType, Type concreteType)
        {
            EnsureNotPreviouslyRegistered(interfaceType);
            EnsureIsConcrete(concreteType);

            _retrievalStrategyStore.Insert(interfaceType, _retrievalStrategyFactory.CreatePerRequest(concreteType));
        }

        private void EnsureNotPreviouslyRegistered(Type interfaceType)
        {
            if (_retrievalStrategyStore.IsRegistered(interfaceType))
            {
                throw ThrowHelper.ServiceFactory.DuplicateRegistration(interfaceType);
            }
        }

        private void EnsureRetrieverHasHandler(Type interfaceType)
        {
            if (!_dependencyRegistrar.HasHandler(interfaceType))
            {
                throw ThrowHelper.ServiceFactory.NoHandlerRegisteredWithContainer(interfaceType);
            }
        }

        private void EnsureIsConcrete(Type concreteType)
        {
            if (concreteType.IsAbstract)
            {
                throw ThrowHelper.ServiceFactory.RegisterNonConcreteTypePerRequest(concreteType);
            }
        }
    }
}