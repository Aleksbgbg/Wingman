namespace Wingman.Tests.ServiceFactory.Strategies.FromRetriever
{
    using System;

    using Moq;

    using Wingman.Container;
    using Wingman.ServiceFactory.Strategies.FromRetriever;

    using Xunit;

    public class FromRetrieverRetrievalStrategyTests
    {
        private readonly Mock<IDependencyRetriever> _dependencyRetrieverMock;

        private readonly FromRetrieverRetrievalStrategy _fromRetrieverRetrievalStrategy;

        public FromRetrieverRetrievalStrategyTests()
        {
            _dependencyRetrieverMock = new Mock<IDependencyRetriever>();

            _fromRetrieverRetrievalStrategy = new FromRetrieverRetrievalStrategy(_dependencyRetrieverMock.Object,
                                                                                 typeof(IService));
        }

        [Fact]
        public void RetrieveServiceThrowsWhenArgumentsNotEmpty()
        {
            Action retrieve = () => RetrieveService(new object[4]);

            Assert.Throws<ArgumentOutOfRangeException>(retrieve);
        }

        [Fact]
        public void RetrieveServiceCallsRetriever()
        {
            RetrieveService();

            VerifyGetInstanceCalled();
        }

        private void RetrieveService(params object[] arguments)
        {
            _fromRetrieverRetrievalStrategy.RetrieveService(arguments);
        }

        private void VerifyGetInstanceCalled()
        {
            _dependencyRetrieverMock.Verify(retriever => retriever.GetInstance(typeof(IService), null));
        }

        private interface IService
        {
        }
    }
}