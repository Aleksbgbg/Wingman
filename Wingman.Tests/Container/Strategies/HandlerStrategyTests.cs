namespace Wingman.Tests.Container.Strategies
{
    using System;

    using Moq;

    using Wingman.Container;
    using Wingman.Container.Strategies;

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

            object actualObject = HandlerStrategy(_ => expectedObject).LocateService();

            Assert.Same(expectedObject, actualObject);
        }

        [Fact]
        public void TestPassesDependencyRetrieverArgument()
        {
            IDependencyRetriever actualRetriever = null;

            HandlerStrategy(passedInRetriever => actualRetriever = passedInRetriever).LocateService();

            Assert.Same(_dependencyRetrieverMock.Object, actualRetriever);
        }

        private HandlerStrategy HandlerStrategy(Func<IDependencyRetriever, object> handler)
        {
            return new HandlerStrategy(_dependencyRetrieverMock.Object, handler);
        }
    }
}