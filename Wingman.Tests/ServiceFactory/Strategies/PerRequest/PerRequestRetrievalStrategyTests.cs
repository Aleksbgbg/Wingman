namespace Wingman.Tests.ServiceFactory.Strategies.PerRequest
{
    using Moq;

    using Wingman.ServiceFactory.Strategies.PerRequest;

    using Xunit;

    public class PerRequestRetrievalStrategyTests
    {
        private readonly Mock<IArgumentBuilder> _argumentBuilderMock;

        private readonly Mock<IArgumentBuilderFactory> _argumentBuilderFactoryMock;

        private readonly Mock<IConstructor> _constructorMock;

        private readonly Mock<IConstructorMap> _constructorMapMock;

        private readonly Mock<IConstructorMapFactory> _constructorMapFactoryMock;

        private readonly PerRequestRetrievalStrategy _perRequestRetrievalStrategy;

        private readonly object[] _userArguments = { new object(), new object() };

        private readonly object[] _resolvedArguments = { new object(), new object() };

        private readonly object _buildResult = new object();

        public PerRequestRetrievalStrategyTests()
        {
            _constructorMock = new Mock<IConstructor>();
            _constructorMock.Setup(constructor => constructor.Build(_resolvedArguments))
                            .Returns(_buildResult);

            _argumentBuilderMock = new Mock<IArgumentBuilder>();
            _argumentBuilderMock.Setup(builder => builder.BuildArguments())
                                .Returns(_resolvedArguments);

            _argumentBuilderFactoryMock = new Mock<IArgumentBuilderFactory>();
            _argumentBuilderFactoryMock.Setup(factory => factory.CreateBuilderFor(_constructorMock.Object, _userArguments))
                                       .Returns(_argumentBuilderMock.Object);

            _constructorMapMock = new Mock<IConstructorMap>();
            _constructorMapMock.Setup(constructorMap => constructorMap.FindBestFitForArguments(_userArguments))
                               .Returns(_constructorMock.Object);

            _constructorMapFactoryMock = new Mock<IConstructorMapFactory>();
            _constructorMapFactoryMock.Setup(factory => factory.MapConstructors(typeof(Service)))
                                      .Returns(_constructorMapMock.Object);

            _perRequestRetrievalStrategy = new PerRequestRetrievalStrategy(_argumentBuilderFactoryMock.Object,
                                                                           _constructorMapFactoryMock.Object,
                                                                           typeof(Service));
        }

        [Fact]
        public void CreateMapsConstructors()
        {
            VerifyMapConstructorsCalled();
        }

        [Fact]
        public void RetrieveServiceRetrievesBestFitConstructor()
        {
            _perRequestRetrievalStrategy.RetrieveService(_userArguments);

            VerifyFindBestFitCalled();
        }

        [Fact]
        public void RetrieveServiceBuildsArgumentsForConstructor()
        {
            _perRequestRetrievalStrategy.RetrieveService(_userArguments);

            VerifyCreateBuilderCalled();
            VerifyBuildArgumentsCalled();
        }

        [Fact]
        public void RetrieveServiceBuildsObjectWithArguments()
        {
            object result = _perRequestRetrievalStrategy.RetrieveService(_userArguments);

            VerifyBuildConstructorCalled();
            Assert.Equal(_buildResult, result);
        }

        private void VerifyMapConstructorsCalled()
        {
            _constructorMapFactoryMock.Verify(factory => factory.MapConstructors(typeof(Service)));
        }

        private void VerifyFindBestFitCalled()
        {
            _constructorMapMock.Verify(constructorMap => constructorMap.FindBestFitForArguments(_userArguments));
        }

        private void VerifyCreateBuilderCalled()
        {
            _argumentBuilderFactoryMock.Verify(factory => factory.CreateBuilderFor(_constructorMock.Object, _userArguments));
        }

        private void VerifyBuildArgumentsCalled()
        {
            _argumentBuilderMock.Verify(builder => builder.BuildArguments());
        }

        private void VerifyBuildConstructorCalled()
        {
            _constructorMock.Verify(constructor => constructor.Build(_resolvedArguments));
        }

        private class Service
        {
        }
    }
}