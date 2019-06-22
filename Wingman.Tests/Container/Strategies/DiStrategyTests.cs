namespace Wingman.Tests.Container.Strategies
{
    using Moq;

    using Wingman.Container.Strategies;
    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    using Xunit;

    public class DiStrategyTests
    {
        private readonly Mock<IObjectBuilder> _objectBuilderMock;

        private readonly DiStrategy _diStrategy;

        public DiStrategyTests()
        {
            Mock<IConstructor> constructorMock = new Mock<IConstructor>();
            Mock<IDiConstructorMap> diConstructorMapMock = new Mock<IDiConstructorMap>();
            diConstructorMapMock.Setup(map => map.FindBestConstructorForDi())
                                .Returns(constructorMock.Object);

            Mock<IArgumentBuilder> argumentBuilderMock = new Mock<IArgumentBuilder>();
            Mock<IDiArgumentBuilderFactory> diArgumentBuilderFactoryMock = new Mock<IDiArgumentBuilderFactory>();
            diArgumentBuilderFactoryMock.Setup(factory => factory.CreateBuilderFor(constructorMock.Object))
                                        .Returns(argumentBuilderMock.Object);

            _objectBuilderMock = new Mock<IObjectBuilder>();
            Mock<IObjectBuilderFactory> objectBuilderFactoryMock = new Mock<IObjectBuilderFactory>();
            objectBuilderFactoryMock.Setup(factory => factory.CreateBuilder(constructorMock.Object, argumentBuilderMock.Object))
                                    .Returns(_objectBuilderMock.Object);

            _diStrategy = new DiStrategy(diConstructorMapMock.Object, diArgumentBuilderFactoryMock.Object, objectBuilderFactoryMock.Object);
        }

        [Fact]
        public void TestLocateService()
        {
            object expectedObject = new object();
            SetupBuildObject(expectedObject);

            object actualObject = _diStrategy.LocateService();

            Assert.Same(expectedObject, actualObject);
        }

        private void SetupBuildObject(object expectedObject)
        {
            _objectBuilderMock.Setup(builder => builder.BuildObject())
                              .Returns(expectedObject);
        }
    }
}