namespace Wingman.Tests.ServiceFactory
{
    using Wingman.ServiceFactory;

    using Xunit;

    public class ServiceRetrievalStrategyStoreTests
    {
        private readonly ServiceRetrievalStrategyStore _serviceRetrievalStrategyStore;

        public ServiceRetrievalStrategyStoreTests()
        {
            _serviceRetrievalStrategyStore = new ServiceRetrievalStrategyStore();
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
        }

        [Fact]
        public void InsertPerRequestCreatesEntry()
        {
            InsertPerRequest();

            Assert.True(IsServiceRegistered());
        }

        [Fact]
        public void RetrieveMappingForRetriever()
        {
            InsertFromRetriever();

            IServiceRetrievalStrategy serviceRetrievalStrategy = RetrieveMapping();

            Assert.IsType<FromRetrieverRetrievalStrategy>(serviceRetrievalStrategy);
        }

        [Fact]
        public void RetrieveMappingForPerRequest()
        {
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

        private interface IService
        {
        }

        private class Service : IService
        {
        }
    }
}