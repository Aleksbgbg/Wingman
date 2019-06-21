namespace Wingman.Tests.DI
{
    using System.Collections.Generic;

    using Moq;

    using Wingman.Container;
    using Wingman.DI;
    using Wingman.Tests.Extensions;
    using Wingman.Tests.Helpers.DI;

    using Xunit;

    public class ArgumentBuilderTests
    {
        private readonly Mock<IDependencyRetriever> _dependencyRetrieverMock;

        private readonly Mock<IConstructor> _constructorMock;

        public ArgumentBuilderTests()
        {
            _dependencyRetrieverMock = new Mock<IDependencyRetriever>();

            _constructorMock = new Mock<IConstructor>();
        }

        [Fact]
        public void ReturnsUserArgumentsWhenEqualLength()
        {
            object[] userArguments = SetupNArgumentsWithNDependencies(3, 0);

            object[] arguments = BuildArguments(userArguments);

            Assert.Same(userArguments, arguments);
        }

        [Fact]
        public void ResolvesDependenciesBasedOnArgumentTypes()
        {
            const int dependencyCount = 3;
            object[] dependencies = SetupDependencies(dependencyCount);
            object[] userArguments = SetupNArgumentsWithNDependencies(4, dependencyCount);

            HashSet<object> arguments = BuildArguments(userArguments).ToHashSetInternal();

            Assert.Subset(arguments, dependencies.ToHashSetInternal());
            Assert.Subset(arguments, userArguments.ToHashSetInternal());
        }

        private object[] SetupNArgumentsWithNDependencies(int count, int dependencies)
        {
            _constructorMock.SetupGet(constructor => constructor.ParameterCount)
                            .Returns(count);

            return new object[count - dependencies];
        }

        private object[] SetupDependencies(int count)
        {
            return DiHelper.SetupDependencies(_constructorMock, _dependencyRetrieverMock, count);
        }

        private object[] BuildArguments(object[] userArguments)
        {
            return new ArgumentBuilder(_dependencyRetrieverMock.Object,
                                       _constructorMock.Object,
                                       userArguments)
                    .BuildArguments();
        }
    }
}