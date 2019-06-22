namespace Wingman.Tests.Helpers.DI
{
    using System.Linq;

    using Moq;

    using Wingman.Container;
    using Wingman.DI.Constructor;

    internal static class DiHelper
    {
        internal static object[] SetupDependencies(Mock<IConstructorParameterInfo> constructorMock, Mock<IDependencyRetriever> dependencyRetrieverMock, int count)
        {
            DependencyType[] dependencies = new DependencyType[count];

            constructorMock.SetupGet(constructor => constructor.ParameterCount)
                           .Returns(count);
            constructorMock.Setup(constructor => constructor.ParameterTypeAt(It.Is<int>(value => 0 <= value && value < count)))
                           .Returns(typeof(DependencyType));

            var sequenceSetup = dependencyRetrieverMock.SetupSequence(retriever => retriever.GetInstance(typeof(DependencyType), null));

            for (int index = 0; index < count; ++index)
            {
                DependencyType dependency = new DependencyType();

                sequenceSetup.Returns(dependency);
                dependencies[index] = dependency;
            }

            return dependencies.Cast<object>().ToArray();
        }

        private class DependencyType { }
    }
}