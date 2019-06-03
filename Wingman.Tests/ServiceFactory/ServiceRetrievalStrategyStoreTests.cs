namespace Wingman.Tests.ServiceFactory
{
    using Moq;

    using Wingman.ServiceFactory;

    using Xunit;

    public class ServiceRetrievalStrategyStoreTests
    {
        private readonly Mock<IRetrievalStrategyFactory> _retrievalStrategyFactoryMock;

        private readonly ServiceRetrievalStrategyStore _serviceRetrievalStrategyStore;

        public ServiceRetrievalStrategyStoreTests()
        {
            _retrievalStrategyFactoryMock = new Mock<IRetrievalStrategyFactory>();

            _serviceRetrievalStrategyStore = new ServiceRetrievalStrategyStore(_retrievalStrategyFactoryMock.Object);
        }

        [Fact]
        public void IsRegisteredFalseForUnregisteredService()
        {
            Assert.False(IsServiceRegistered());
        }

        [Fact]
        public void InsertFromRetrieverCreatesEntry()
        {
            InsertFromRetriever();

            Assert.True(IsServiceRegistered());
            VerifyFromRetrieverCalled();
        }

        [Fact]
        public void InsertPerRequestCreatesEntry()
        {
            InsertPerRequest();

            Assert.True(IsServiceRegistered());
            VerifyPerRequestCalled();
        }

        [Fact]
        public void RetrieveMappingForRetriever()
        {
            SetupFromRetriever();
            InsertFromRetriever();

            IServiceRetrievalStrategy serviceRetrievalStrategy = RetrieveMapping();

            Assert.IsType<FromRetrieverRetrievalStrategy>(serviceRetrievalStrategy);
        }

        [Fact]
        public void RetrieveMappingForPerRequest()
        {
            SetupPerRequest();
            InsertPerRequest();

            IServiceRetrievalStrategy serviceRetrievalStrategy = RetrieveMapping();

            Assert.IsType<PerRequestRetrievalStrategy>(serviceRetrievalStrategy);
        }

        private bool IsServiceRegistered()
        {
            return _serviceRetrievalStrategyStore.IsRegistered(typeof(IService));
        }

        private void InsertFromRetriever()
        {
            _serviceRetrievalStrategyStore.InsertFromRetriever(typeof(IService));
        }

        private void InsertPerRequest()
        {
            _serviceRetrievalStrategyStore.InsertPerRequest(typeof(IService), typeof(Service));
        }

        private IServiceRetrievalStrategy RetrieveMapping()
        {
            return _serviceRetrievalStrategyStore.RetrieveMappingFor(typeof(IService));
        }

        private void SetupFromRetriever()
        {
            _retrievalStrategyFactoryMock.Setup(factory => factory.FromRetriever(typeof(IService)))
                                         .Returns(new FromRetrieverRetrievalStrategy(null, null));
        }

        private void SetupPerRequest()
        {
            _retrievalStrategyFactoryMock.Setup(factory => factory.PerRequest(typeof(IService), typeof(Service)))
                                         .Returns(new PerRequestRetrievalStrategy());
        }

        private void VerifyFromRetrieverCalled()
        {
            _retrievalStrategyFactoryMock.Verify(factory => factory.FromRetriever(typeof(IService)));
        }

        private void VerifyPerRequestCalled()
        {
            _retrievalStrategyFactoryMock.Verify(factory => factory.PerRequest(typeof(IService), typeof(Service)));
        }

        private interface IService
        {
        }

        private class Service : IService
        {
        }
    }
}