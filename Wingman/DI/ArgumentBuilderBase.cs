namespace Wingman.DI
{
    using Wingman.Container;

    internal abstract class ArgumentBuilderBase : IArgumentBuilder
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        private readonly IConstructor _constructor;

        private protected ArgumentBuilderBase(IDependencyRetriever dependencyRetriever, IConstructor constructor)
        {
            _dependencyRetriever = dependencyRetriever;
            _constructor = constructor;
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
                Arguments[dependencyIndex] = _dependencyRetriever.GetInstance(_constructor.ParameterTypeAt(dependencyIndex));
            }
        }
    }
}