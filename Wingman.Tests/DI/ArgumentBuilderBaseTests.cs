namespace Wingman.Tests.DI
{
    using Moq;

    using Wingman.Container;
    using Wingman.DI;
    using Wingman.Tests.Helpers.DI;

    using Xunit;

    public class ArgumentBuilderBaseTests
    {
        private readonly Mock<IDependencyRetriever> _dependencyRetrieverMock;

        private readonly Mock<IConstructor> _constructorMock;

        public ArgumentBuilderBaseTests()
        {
            _dependencyRetrieverMock = new Mock<IDependencyRetriever>();

            _constructorMock = new Mock<IConstructor>();
        }

        [Fact]
        public void ResolvesDependenciesBasedOnArgumentTypes()
        {
            object[] dependencies = SetupDependencies(3);

            object[] arguments = ResolveDependencies();

            Assert.Equal(dependencies, arguments);
        }

        private object[] SetupDependencies(int count)
        {
            return DiHelper.SetupDependencies(_constructorMock, _dependencyRetrieverMock, count);
        }

        private object[] ResolveDependencies()
        {
            return new ArgumentBuilderBaseMock(_dependencyRetrieverMock.Object, _constructorMock.Object).BuildArguments();
        }

        private class ArgumentBuilderBaseMock : ArgumentBuilderBase
        {
            private readonly IConstructor _constructor;

            internal ArgumentBuilderBaseMock(IDependencyRetriever dependencyRetriever, IConstructor constructor) : base(dependencyRetriever, constructor)
            {
                _constructor = constructor;
            }

            private protected override void InstantiateAndFillArguments()
            {
                Arguments = new object[_constructor.ParameterCount];
                ResolveDependenciesFromStart(Arguments.Length);
            }
        }
    }
}