﻿namespace Wingman.Tests.ServiceFactory.Strategies
{
    using Moq;

    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;
    using Wingman.ServiceFactory.Strategies;

    using Xunit;

    public class PerRequestRetrievalStrategyTests
    {
        private readonly Mock<IConstructor> _constructorMock;

        private readonly Mock<IArgumentBuilder> _argumentBuilderMock;

        private readonly Mock<IObjectBuilderFactory> _objectBuilderFactoryMock;

        private readonly PerRequestRetrievalStrategy _perRequestRetrievalStrategy;

        private readonly object[] _userArguments = { new object(), new object() };

        public PerRequestRetrievalStrategyTests()
        {
            _constructorMock = new Mock<IConstructor>();

            _argumentBuilderMock = new Mock<IArgumentBuilder>();

            Mock<IArgumentConstructorMap> argumentConstructorMapMock = new Mock<IArgumentConstructorMap>();
            argumentConstructorMapMock.Setup(map => map.FindBestConstructorForArguments(_userArguments))
                                      .Returns(_constructorMock.Object);

            Mock<IUserArgumentBuilderFactory> userArgumentBuilderFactoryMock = new Mock<IUserArgumentBuilderFactory>();
            userArgumentBuilderFactoryMock.Setup(factory => factory.CreateBuilderFor(_constructorMock.Object, _userArguments))
                                          .Returns(_argumentBuilderMock.Object);

            _objectBuilderFactoryMock = new Mock<IObjectBuilderFactory>();

            _perRequestRetrievalStrategy = new PerRequestRetrievalStrategy(argumentConstructorMapMock.Object,
                                                                           userArgumentBuilderFactoryMock.Object,
                                                                           _objectBuilderFactoryMock.Object);
        }

        [Fact]
        public void TestBuildsObject()
        {
            object expectedService = new object();
            SetupBuildObject(expectedService);

            object actualService = RetrieveService();

            Assert.Same(expectedService, actualService);
        }

        private void SetupBuildObject(object service)
        {
            Mock<IObjectBuilder> objectBuilderMock = new Mock<IObjectBuilder>();
            objectBuilderMock.Setup(builder => builder.BuildObject())
                             .Returns(service);

            _objectBuilderFactoryMock.Setup(factory => factory.CreateBuilder(_constructorMock.Object, _argumentBuilderMock.Object))
                                     .Returns(objectBuilderMock.Object);
        }

        private object RetrieveService()
        {
            return _perRequestRetrievalStrategy.RetrieveService(_userArguments);
        }
    }
}