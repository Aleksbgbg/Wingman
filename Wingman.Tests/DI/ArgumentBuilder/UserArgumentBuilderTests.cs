﻿namespace Wingman.Tests.DI.ArgumentBuilder
{
    using System.Collections.Generic;

    using Moq;

    using Wingman.Container;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;
    using Wingman.Tests.Extensions;
    using Wingman.Tests.Helpers.DI;

    using Xunit;

    public class UserArgumentBuilderTests
    {
        private readonly Mock<IDependencyRetriever> _dependencyRetrieverMock;

        private readonly Mock<IConstructorParameterInfo> _constructorParameterInfoMock;

        public UserArgumentBuilderTests()
        {
            _dependencyRetrieverMock = new Mock<IDependencyRetriever>();

            _constructorParameterInfoMock = new Mock<IConstructorParameterInfo>();
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
            _constructorParameterInfoMock.SetupGet(constructor => constructor.ParameterCount)
                            .Returns(count);

            return new object[count - dependencies];
        }

        private object[] SetupDependencies(int count)
        {
            return DiHelper.SetupDependencies(_constructorParameterInfoMock, _dependencyRetrieverMock, count);
        }

        private object[] BuildArguments(object[] userArguments)
        {
            return new UserArgumentBuilder(_dependencyRetrieverMock.Object,
                                           _constructorParameterInfoMock.Object,
                                           userArguments)
                    .BuildArguments();
        }
    }
}