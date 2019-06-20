namespace Wingman.Tests.Container.Strategies
{
    using System;

    using Moq;

    using Wingman.Container.Strategies;
    using Wingman.DI;

    using Xunit;

    public class DiStrategyTests
    {
        private static readonly Type TargetType = typeof(RequestType);

        private readonly Mock<IConstructorCandidateEvaluator> _constructorEvaluatorMock;

        private readonly Mock<IArgumentBuilderFactory> _argumentBuilderFactoryMock;

        private Mock<IConstructor> _constructorMock;

        public DiStrategyTests()
        {
            _constructorEvaluatorMock = new Mock<IConstructorCandidateEvaluator>();

            _argumentBuilderFactoryMock = new Mock<IArgumentBuilderFactory>();
        }

        [Fact]
        public void TestEvaluatesBestConstructor()
        {
            MakeStrategy();

            VerifyFindBestConstructor();
        }

        [Fact]
        public void TestCreatesArgumentBuilderForConstructor()
        {
            IConstructor constructor = SetupConstructor();

            MakeStrategy();

            VerifyCreateArgumentBuilderFor(constructor);
        }

        [Fact]
        public void TestLocateBuildsArgumentsAndBuildsConstructor()
        {
            object[] arguments = { "1", 2, 3.0f };
            SetupConstructorWithArguments(arguments);

            MakeStrategy().LocateService();

            VerifyBuildConstructorWithArguments(arguments);
        }

        [Fact]
        public void TestLocateReturnsBuildResult()
        {
            object expectedService = new object();
            SetupConstructorBuilds(expectedService);

            object actualService = MakeStrategy().LocateService();

            Assert.Same(expectedService, actualService);
        }

        private DiStrategy MakeStrategy()
        {
            return new DiStrategy(_constructorEvaluatorMock.Object, _argumentBuilderFactoryMock.Object, TargetType);
        }

        private IConstructor SetupConstructor()
        {
            _constructorMock = new Mock<IConstructor>();

            _constructorEvaluatorMock.Setup(evaluator => evaluator.FindBestConstructorForDi(TargetType))
                                     .Returns(_constructorMock.Object);

            return _constructorMock.Object;
        }

        private void SetupConstructorWithArguments(object[] arguments)
        {
            IConstructor constructor = SetupConstructor();

            Mock<IArgumentBuilder> argumentBuilderMock = new Mock<IArgumentBuilder>();
            argumentBuilderMock.Setup(builder => builder.BuildArguments())
                               .Returns(arguments);

            _argumentBuilderFactoryMock.Setup(factory => factory.CreateBuilderFor(constructor))
                                       .Returns(argumentBuilderMock.Object);
        }

        private void SetupConstructorBuilds(object obj)
        {
            SetupConstructorWithArguments(It.IsAny<object[]>());

            _constructorMock.Setup(constructor => constructor.Build(It.IsAny<object[]>()))
                            .Returns(obj);
        }

        private void VerifyFindBestConstructor()
        {
            _constructorEvaluatorMock.Verify(evaluator => evaluator.FindBestConstructorForDi(TargetType));
        }

        private void VerifyCreateArgumentBuilderFor(IConstructor constructor)
        {
            _argumentBuilderFactoryMock.Verify(factory => factory.CreateBuilderFor(constructor));
        }

        private void VerifyBuildConstructorWithArguments(object[] arguments)
        {
            _constructorMock.Verify(constructor => constructor.Build(arguments));
        }

        private class RequestType { }
    }
}