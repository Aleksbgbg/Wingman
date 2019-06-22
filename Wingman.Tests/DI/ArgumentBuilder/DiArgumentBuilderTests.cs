namespace Wingman.Tests.DI.ArgumentBuilder
{
    using Moq;

    using Wingman.Container;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;
    using Wingman.Tests.Helpers.DI;

    using Xunit;

    public class DiArgumentBuilderTests
    {
        private readonly Mock<IDependencyRetriever> _dependencyRetrieverMock;

        private readonly Mock<IConstructorParameterInfo> _constructorParameterInfoMock;

        public DiArgumentBuilderTests()
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
            return new DiArgumentBuilder(_dependencyRetrieverMock.Object, _constructorParameterInfoMock.Object).BuildArguments();
        }
    }
}