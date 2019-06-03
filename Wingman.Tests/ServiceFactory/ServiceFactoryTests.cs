namespace Wingman.Tests.ServiceFactory
{
    using System;

    using Moq;

    using Wingman.Container;
    using Wingman.ServiceFactory;

    using Xunit;

    public class ServiceFactoryTests
    {
        private readonly Mock<IDependencyRegistrar> _dependencyRegistrarMock;

        private readonly Mock<IServiceRetrievalStrategyStore> _serviceRetrievalStrategyStore;

        private readonly ServiceFactory _serviceFactory;

        private Mock<IServiceRetrievalStrategy> _serviceRetrievalStrategyMock;

        public ServiceFactoryTests()
        {
            _dependencyRegistrarMock = new Mock<IDependencyRegistrar>();

            _serviceRetrievalStrategyStore = new Mock<IServiceRetrievalStrategyStore>();

            _serviceFactory = new ServiceFactory(_dependencyRegistrarMock.Object,
                                                 _serviceRetrievalStrategyStore.Object);
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

            VerifyInsertFromRetrieverCalled();
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

            VerifyInsertPerRequestCalled();
        }

        [Fact]
        public void MakeThrowsIfNotRegistered()
        {
            Action make = () => _serviceFactory.Make<IService>();

            Assert.Throws<InvalidOperationException>(make);
            VerifyIsRegisteredCalled();
        }

        [Fact]
        public void MakeRetrievesServiceRetrievalStrategyFromStore()
        {
            SetupServiceIsRegistered();
            SetupServiceRetrievalStrategy(null);

            _serviceFactory.Make<IService>();

            VerifyRetrieveMappingCalled();
        }

        [Fact]
        public void MakeCreatesServiceFromStrategy()
        {
            IService service = new Service();
            SetupServiceIsRegistered();
            SetupServiceRetrievalStrategy(service);

            IService createdService = _serviceFactory.Make<IService>();

            Assert.Equal(service, createdService);
            VerifyRetrieveServiceCalledOnStrategy();
        }

        private void SetupHasServiceHandler()
        {
            _dependencyRegistrarMock.Setup(registrar => registrar.HasHandler(typeof(IService), null))
                                    .Returns(true);
        }

        private void SetupServiceIsRegistered()
        {
            _serviceRetrievalStrategyStore.Setup(store => store.IsRegistered(typeof(IService)))
                                          .Returns(true);
        }

        private void SetupServiceRetrievalStrategy(IService service)
        {
            _serviceRetrievalStrategyMock = new Mock<IServiceRetrievalStrategy>();
            _serviceRetrievalStrategyMock.Setup(strategy => strategy.RetrieveService(It.IsAny<object[]>()))
                                         .Returns(service);

            _serviceRetrievalStrategyStore.Setup(store => store.RetrieveMappingFor(typeof(IService)))
                                          .Returns(_serviceRetrievalStrategyMock.Object);
        }

        private void VerifyHasHandlerCalled()
        {
            _dependencyRegistrarMock.Verify(registrar => registrar.HasHandler(typeof(IService), null));
        }

        private void VerifyInsertFromRetrieverCalled()
        {
            _serviceRetrievalStrategyStore.Verify(store => store.InsertFromRetriever(typeof(IService)));
        }

        private void VerifyInsertPerRequestCalled()
        {
            _serviceRetrievalStrategyStore.Verify(store => store.InsertPerRequest(typeof(IService), typeof(Service)));
        }

        private void VerifyIsRegisteredCalled()
        {
            _serviceRetrievalStrategyStore.Verify(store => store.IsRegistered(typeof(IService)));
        }

        private void VerifyRetrieveMappingCalled()
        {
            _serviceRetrievalStrategyStore.Verify(store => store.RetrieveMappingFor(typeof(IService)));
        }

        private void VerifyRetrieveServiceCalledOnStrategy()
        {
            _serviceRetrievalStrategyMock.Verify(strategy => strategy.RetrieveService(It.IsAny<object[]>()));
        }

        private interface IService
        {
        }

        private class Service : IService
        {
        }
    }
}