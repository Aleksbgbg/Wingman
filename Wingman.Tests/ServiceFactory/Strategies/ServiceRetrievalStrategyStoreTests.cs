namespace Wingman.Tests.ServiceFactory.Strategies
{
    using Moq;

    using Wingman.ServiceFactory.Strategies;

    using Xunit;

    public class RetrievalStrategyStoreTests
    {
        private readonly Mock<IRetrievalStrategyFactory> _retrievalStrategyFactoryMock;

        private readonly RetrievalStrategyStore _retrievalStrategyStore;

        private IServiceRetrievalStrategy _serviceRetrievalStrategy;

        public RetrievalStrategyStoreTests()
        {
            _retrievalStrategyFactoryMock = new Mock<IRetrievalStrategyFactory>();

            _retrievalStrategyStore = new RetrievalStrategyStore(_retrievalStrategyFactoryMock.Object);
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

            Assert.Equal(_serviceRetrievalStrategy, serviceRetrievalStrategy);
        }

        [Fact]
        public void RetrieveMappingForPerRequest()
        {
            SetupPerRequest();
            InsertPerRequest();

            IServiceRetrievalStrategy serviceRetrievalStrategy = RetrieveMapping();

            Assert.Equal(_serviceRetrievalStrategy, serviceRetrievalStrategy);
        }

        private bool IsServiceRegistered()
        {
            return _retrievalStrategyStore.IsRegistered(typeof(IService));
        }

        private void InsertFromRetriever()
        {
            _retrievalStrategyStore.InsertFromRetriever(typeof(IService));
        }

        private void InsertPerRequest()
        {
            _retrievalStrategyStore.InsertPerRequest(typeof(IService), typeof(Service));
        }

        private IServiceRetrievalStrategy RetrieveMapping()
        {
            return _retrievalStrategyStore.RetrieveMappingFor(typeof(IService));
        }

        private void SetupFromRetriever()
        {
            _retrievalStrategyFactoryMock.Setup(factory => factory.FromRetriever(typeof(IService)))
                                         .Returns(CreateServiceRetrievalStrategy());
        }

        private void SetupPerRequest()
        {
            _retrievalStrategyFactoryMock.Setup(factory => factory.PerRequest(typeof(Service)))
                                         .Returns(CreateServiceRetrievalStrategy());
        }

        private IServiceRetrievalStrategy CreateServiceRetrievalStrategy()
        {
            _serviceRetrievalStrategy = new Mock<IServiceRetrievalStrategy>().Object;

            return _serviceRetrievalStrategy;
        }

        private void VerifyFromRetrieverCalled()
        {
            _retrievalStrategyFactoryMock.Verify(factory => factory.FromRetriever(typeof(IService)));
        }

        private void VerifyPerRequestCalled()
        {
            _retrievalStrategyFactoryMock.Verify(factory => factory.PerRequest(typeof(Service)));
        }

        private interface IService
        {
        }

        private class Service : IService
        {
        }
    }
}