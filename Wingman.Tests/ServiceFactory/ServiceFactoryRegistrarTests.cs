namespace Wingman.Tests.ServiceFactory
{
    using System;

    using Moq;

    using Wingman.Container;
    using Wingman.ServiceFactory;
    using Wingman.ServiceFactory.Strategies;

    using Xunit;

    public class ServiceFactoryRegistrarTests
    {
        private readonly Mock<IServiceRetrievalStrategy> _fromRetrieverRetrievalStrategy;

        private readonly Mock<IServiceRetrievalStrategy> _perRequestRetrievalStrategy;

        private readonly Mock<IDependencyRegistrar> _dependencyRegistrarMock;

        private readonly Mock<IRetrievalStrategyStore> _retrievalStrategyStore;

        private readonly ServiceFactoryRegistrar _serviceFactory;

        public ServiceFactoryRegistrarTests()
        {
            _fromRetrieverRetrievalStrategy = new Mock<IServiceRetrievalStrategy>();

            _perRequestRetrievalStrategy = new Mock<IServiceRetrievalStrategy>();

            _dependencyRegistrarMock = new Mock<IDependencyRegistrar>();

            Mock<IRetrievalStrategyFactory> retrievalStrategyFactoryMock = new Mock<IRetrievalStrategyFactory>();
            retrievalStrategyFactoryMock.Setup(factory => factory.CreateFromRetriever(typeof(IService)))
                                        .Returns(_fromRetrieverRetrievalStrategy.Object);
            retrievalStrategyFactoryMock.Setup(factory => factory.CreatePerRequest(typeof(Service)))
                                        .Returns(_perRequestRetrievalStrategy.Object);

            _retrievalStrategyStore = new Mock<IRetrievalStrategyStore>();

            _serviceFactory = new ServiceFactoryRegistrar(_dependencyRegistrarMock.Object,
                                                          retrievalStrategyFactoryMock.Object,
                                                          _retrievalStrategyStore.Object);
        }

        [Fact]
        public void RegisterFromRetrieverThrowsWhenNoHandlerRegistered()
        {
            Action register = () => _serviceFactory.RegisterFromRetriever<IService>();

            Assert.Throws<InvalidOperationException>(register);
            VerifyHasHandlerCalled();
        }

        [Fact]
        public void RegisterFromRetrieverThrowsWhenDuplicateRegistered()
        {
            SetupServiceIsRegistered();
            SetupHasServiceHandler();

            Action register = () => _serviceFactory.RegisterFromRetriever<IService>();

            Assert.Throws<InvalidOperationException>(register);
            VerifyIsRegisteredCalled();
        }

        [Fact]
        public void RegisterFromRetrieverInsertsIntoStore()
        {
            SetupHasServiceHandler();

            _serviceFactory.RegisterFromRetriever<IService>();

            VerifyInsertFromRetrieverStrategyCalled();
        }

        [Fact]
        public void RegisterPerRequestThrowsWhenDuplicateRegistered()
        {
            SetupServiceIsRegistered();

            Action register = () => _serviceFactory.RegisterPerRequest<IService, Service>();

            Assert.Throws<InvalidOperationException>(register);
            VerifyIsRegisteredCalled();
        }

        [Fact]
        public void RegisterPerRequestThrowsWhenConcreteTypeIsNotConcrete()
        {
            Action register = () => _serviceFactory.RegisterPerRequest<IService, IService>();

            Assert.Throws<InvalidOperationException>(register);
        }

        [Fact]
        public void RegisterPerRequestInsertsIntoStore()
        {
            _serviceFactory.RegisterPerRequest<IService, Service>();

            VerifyInsertPerRequestStrategyCalled();
        }

        private void SetupHasServiceHandler()
        {
            _dependencyRegistrarMock.Setup(registrar => registrar.HasHandler(typeof(IService), null))
                                    .Returns(true);
        }

        private void SetupServiceIsRegistered()
        {
            _retrievalStrategyStore.Setup(store => store.IsRegistered(typeof(IService)))
                                          .Returns(true);
        }

        private void VerifyHasHandlerCalled()
        {
            _dependencyRegistrarMock.Verify(registrar => registrar.HasHandler(typeof(IService), null));
        }

        private void VerifyInsertFromRetrieverStrategyCalled()
        {
            _retrievalStrategyStore.Verify(store => store.Insert(typeof(IService), _fromRetrieverRetrievalStrategy.Object));
        }

        private void VerifyInsertPerRequestStrategyCalled()
        {
            _retrievalStrategyStore.Verify(store => store.Insert(typeof(IService), _perRequestRetrievalStrategy.Object));
        }

        private void VerifyIsRegisteredCalled()
        {
            _retrievalStrategyStore.Verify(store => store.IsRegistered(typeof(IService)));
        }

        private interface IService { }

        private class Service : IService { }
    }
}