namespace Wingman.Tests.Container
{
    using Moq;

    using Wingman.Container;

    using Xunit;

    public class HandlerStrategyTests
    {
        private readonly Mock<IDependencyRetriever> _dependencyRetrieverMock;

        public HandlerStrategyTests()
        {
            _dependencyRetrieverMock = new Mock<IDependencyRetriever>();
        }

        [Fact]
        public void TestReturnsHandlerResult()
        {
            object expectedObject = new object();

            object actualObject = new HandlerStrategy(_ => expectedObject).LocateService(null);

            Assert.Same(expectedObject, actualObject);
        }

        [Fact]
        public void TestPassesDependencyRetrieverArgument()
        {
            IDependencyRetriever actualRetriever = null;

            new HandlerStrategy(passedInRetriever => actualRetriever = passedInRetriever).LocateService(_dependencyRetrieverMock.Object);

            Assert.Same(_dependencyRetrieverMock.Object, actualRetriever);
        }
    }
}