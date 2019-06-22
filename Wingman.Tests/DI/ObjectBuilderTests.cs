namespace Wingman.Tests.DI
{
    using Moq;

    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    using Xunit;

    public class ObjectBuilderTests
    {
        private readonly Mock<IConstructorBuilder> _constructorBuilderMock;

        private readonly Mock<IArgumentBuilder> _argumentBuilderMock;

        private readonly ObjectBuilder _objectBuilder;

        public ObjectBuilderTests()
        {
            _constructorBuilderMock = new Mock<IConstructorBuilder>();

            _argumentBuilderMock = new Mock<IArgumentBuilder>();

            _objectBuilder = new ObjectBuilder(_constructorBuilderMock.Object, _argumentBuilderMock.Object);
        }

        [Fact]
        public void TestBuildsConstructorWithBuiltArguments()
        {
            object expectedObject = new object();
            object[] arguments = { new SomeType(), new SomeType() };
            SetupBuildObject(arguments, expectedObject);

            object builtObject = _objectBuilder.BuildObject();

            Assert.Same(expectedObject, builtObject);
        }

        private void SetupBuildObject(object[] arguments, object value)
        {
            _argumentBuilderMock.Setup(builder => builder.BuildArguments())
                                .Returns(arguments);

            _constructorBuilderMock.Setup(constructor => constructor.BuildWith(arguments))
                                   .Returns(value);
        }

        private class SomeType { }
    }
}