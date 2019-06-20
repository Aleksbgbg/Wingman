namespace Wingman.Tests.ServiceFactory.Strategies
{
    using Moq;

    using Wingman.DI;
    using Wingman.ServiceFactory.Strategies;
    using Wingman.ServiceFactory.Strategies.FromRetriever;
    using Wingman.ServiceFactory.Strategies.PerRequest;

    using Xunit;

    public class RetrievalStrategyFactoryTests
    {
        private readonly RetrievalStrategyFactory _retrievalStrategyFactory;

        public RetrievalStrategyFactoryTests()
        {
            _retrievalStrategyFactory = new RetrievalStrategyFactory(null, null, new Mock<IConstructorMapFactory>().Object);
        }

        [Fact]
        public void FromRetrieverReturnsCorrectType()
        {
            IServiceRetrievalStrategy strategy = _retrievalStrategyFactory.CreateFromRetriever(typeof(IService));

            Assert.IsType<FromRetrieverRetrievalStrategy>(strategy);
        }

        [Fact]
        public void PerRequestReturnsCorrectType()
        {
            IServiceRetrievalStrategy strategy = _retrievalStrategyFactory.CreatePerRequest(typeof(Service));

            Assert.IsType<PerRequestRetrievalStrategy>(strategy);
        }

        private interface IService
        {
        }

        private class Service : IService
        {
        }
    }
}