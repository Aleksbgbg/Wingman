namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.Container;
    using Wingman.DI.Constructor;

    internal abstract class ArgumentBuilderBase : IArgumentBuilder
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        private readonly IConstructorParameterInfo _constructorParameterInfo;

        private protected ArgumentBuilderBase(IDependencyRetriever dependencyRetriever, IConstructorParameterInfo constructorParameterInfo)
        {
            _dependencyRetriever = dependencyRetriever;
            _constructorParameterInfo = constructorParameterInfo;
        }

        private protected object[] Arguments { get; set; }

        public object[] BuildArguments()
        {
            InstantiateAndFillArguments();
            return Arguments;
        }

        private protected abstract void InstantiateAndFillArguments();

        private protected void ResolveDependenciesFromStart(int dependencyCount)
        {
            for (int dependencyIndex = 0; dependencyIndex < dependencyCount; ++dependencyIndex)
            {
                Arguments[dependencyIndex] = _dependencyRetriever.GetInstance(_constructorParameterInfo.ParameterTypeAt(dependencyIndex));
            }
        }
    }
}