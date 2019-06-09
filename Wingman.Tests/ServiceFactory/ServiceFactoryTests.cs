namespace Wingman.Tests.ServiceFactory
{
    using System;

    using Moq;

    using Wingman.ServiceFactory;
    using Wingman.ServiceFactory.Strategies;

    using Xunit;

    public class ServiceFactoryTests
    {
        private readonly Mock<IRetrievalStrategyStore> _retrievalStrategyStore;

        private readonly ServiceFactory _serviceFactory;

        private Mock<IServiceRetrievalStrategy> _serviceRetrievalStrategyMock;

        public ServiceFactoryTests()
        {
            _retrievalStrategyStore = new Mock<IRetrievalStrategyStore>();

            _serviceFactory = new ServiceFactory(_retrievalStrategyStore.Object);
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

        private void SetupServiceIsRegistered()
        {
            _retrievalStrategyStore.Setup(store => store.IsRegistered(typeof(IService)))
                                          .Returns(true);
        }

        private void SetupServiceRetrievalStrategy(IService service)
        {
            _serviceRetrievalStrategyMock = new Mock<IServiceRetrievalStrategy>();
            _serviceRetrievalStrategyMock.Setup(strategy => strategy.RetrieveService(It.IsAny<object[]>()))
                                         .Returns(service);

            _retrievalStrategyStore.Setup(store => store.RetrieveMappingFor(typeof(IService)))
                                          .Returns(_serviceRetrievalStrategyMock.Object);
        }

        private void VerifyIsRegisteredCalled()
        {
            _retrievalStrategyStore.Verify(store => store.IsRegistered(typeof(IService)));
        }

        private void VerifyRetrieveMappingCalled()
        {
            _retrievalStrategyStore.Verify(store => store.RetrieveMappingFor(typeof(IService)));
        }

        private void VerifyRetrieveServiceCalledOnStrategy()
        {
            _serviceRetrievalStrategyMock.Verify(strategy => strategy.RetrieveService(It.IsAny<object[]>()));
        }

        private interface IService { }

        private class Service : IService { }
    }
}