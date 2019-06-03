namespace Wingman.Tests.ServiceFactory
{
    using Moq;

    using Wingman.ServiceFactory;

    using Xunit;

    public class PerRequestRetrievalStrategyTests
    {
        private readonly Mock<IArgumentBuilder> _argumentBuilderMock;

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
            _argumentBuilderMock.Setup(builder => builder.BuildArgumentsForConstructor(_constructorMock.Object, _userArguments))
                                .Returns(_resolvedArguments);

            _constructorMapMock = new Mock<IConstructorMap>();
            _constructorMapMock.Setup(constructorMap => constructorMap.FindBestFitForArguments(_userArguments))
                               .Returns(_constructorMock.Object);

            _constructorMapFactoryMock = new Mock<IConstructorMapFactory>();
            _constructorMapFactoryMock.Setup(factory => factory.MapConstructors(typeof(Service)))
                                      .Returns(_constructorMapMock.Object);

            _perRequestRetrievalStrategy = new PerRequestRetrievalStrategy(_argumentBuilderMock.Object,
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

            VerifyBuildArgumentsForConstructorCalled();
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

        private void VerifyBuildArgumentsForConstructorCalled()
        {
            _argumentBuilderMock.Verify(builder => builder.BuildArgumentsForConstructor(_constructorMock.Object, _userArguments));
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