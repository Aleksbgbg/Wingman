namespace Wingman.Tests.ServiceFactory.Strategies
{
    using Moq;

    using Wingman.ServiceFactory.Strategies;

    using Xunit;

    public class RetrievalStrategyStoreTests
    {
        private readonly RetrievalStrategyStore _retrievalStrategyStore;

        private IServiceRetrievalStrategy _serviceRetrievalStrategy;

        public RetrievalStrategyStoreTests()
        {
            _retrievalStrategyStore = new RetrievalStrategyStore();
        }

        [Fact]
        public void IsRegisteredFalseForUnregisteredService()
        {
            Assert.False(IsServiceRegistered());
        }

        [Fact]
        public void TestInsert()
        {
            InsertMapping();

            Assert.True(IsServiceRegistered());
        }

        [Fact]
        public void TestRetrieveMapping()
        {
            InsertMapping();

            IServiceRetrievalStrategy serviceRetrievalStrategy = RetrieveMapping();

            Assert.Equal(_serviceRetrievalStrategy, serviceRetrievalStrategy);
        }

        private bool IsServiceRegistered()
        {
            return _retrievalStrategyStore.IsRegistered(typeof(IService));
        }

        private void InsertMapping()
        {
            _retrievalStrategyStore.Insert(typeof(IService), CreateServiceRetrievalStrategy());
        }

        private IServiceRetrievalStrategy RetrieveMapping()
        {
            return _retrievalStrategyStore.RetrieveMappingFor(typeof(IService));
        }

        private IServiceRetrievalStrategy CreateServiceRetrievalStrategy()
        {
            _serviceRetrievalStrategy = new Mock<IServiceRetrievalStrategy>().Object;

            return _serviceRetrievalStrategy;
        }

        private interface IService { }
    }
}