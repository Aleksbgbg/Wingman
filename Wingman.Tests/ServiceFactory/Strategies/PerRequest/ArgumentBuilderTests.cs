namespace Wingman.Tests.ServiceFactory.Strategies.PerRequest
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using Wingman.Container;
    using Wingman.ServiceFactory.Strategies.PerRequest;

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
            object[] userArguments = SetupNArgumentsWithNDependencies(4, dependencyCount);
            object[] dependencies = SetupDependencies(dependencyCount).Cast<object>().ToArray();

            HashSet<object> arguments = BuildArguments(userArguments).ToHashSet();

            Assert.Subset(arguments, dependencies.ToHashSet());
            Assert.Subset(arguments, userArguments.ToHashSet());
        }

        private object[] SetupNArgumentsWithNDependencies(int count, int dependencies)
        {
            _constructorMock.SetupGet(constructor => constructor.ParameterCount)
                            .Returns(count);

            return new object[count - dependencies];
        }

        private DependencyType[] SetupDependencies(int count)
        {
            DependencyType[] dependencies = new DependencyType[count];

            var sequenceSetup = _dependencyRetrieverMock.SetupSequence(retriever => retriever.GetInstance(typeof(DependencyType), null));

            for (int index = 0; index < count; ++index)
            {
                DependencyType dependency = new DependencyType();

                _constructorMock.Setup(constructor => constructor.ParameterTypeAt(index))
                                .Returns(typeof(DependencyType));

                sequenceSetup.Returns(dependency);
                dependencies[index] = dependency;
            }

            return dependencies;
        }

        private object[] BuildArguments(object[] userArguments)
        {
            return new ArgumentBuilder(_dependencyRetrieverMock.Object,
                                       _constructorMock.Object,
                                       userArguments)
                    .BuildArguments();
        }

        private class DependencyType { }
    }
}