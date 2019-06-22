namespace Wingman.Tests.DI.ArgumentBuilder
{
    using Moq;

    using Wingman.Container;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;
    using Wingman.Tests.Helpers.DI;

    using Xunit;

    public class ArgumentBuilderBaseTests
    {
        private readonly Mock<IDependencyRetriever> _dependencyRetrieverMock;

        private readonly Mock<IConstructorParameterInfo> _constructorParameterInfoMock;

        public ArgumentBuilderBaseTests()
        {
            _dependencyRetrieverMock = new Mock<IDependencyRetriever>();

            _constructorParameterInfoMock = new Mock<IConstructorParameterInfo>();
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
            return DiHelper.SetupDependencies(_constructorParameterInfoMock, _dependencyRetrieverMock, count);
        }

        private object[] ResolveDependencies()
        {
            return new ArgumentBuilderBaseMock(_dependencyRetrieverMock.Object, _constructorParameterInfoMock.Object).BuildArguments();
        }

        private class ArgumentBuilderBaseMock : ArgumentBuilderBase
        {
            private readonly IConstructorParameterInfo _constructorParameterInfo;

            internal ArgumentBuilderBaseMock(IDependencyRetriever dependencyRetriever, IConstructorParameterInfo constructorParameterInfo) : base(dependencyRetriever, constructorParameterInfo)
            {
                _constructorParameterInfo = constructorParameterInfo;
            }

            private protected override void InstantiateAndFillArguments()
            {
                Arguments = new object[_constructorParameterInfo.ParameterCount];
                ResolveDependenciesFromStart(Arguments.Length);
            }
        }
    }
}