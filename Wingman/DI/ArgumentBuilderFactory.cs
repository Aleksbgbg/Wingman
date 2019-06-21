namespace Wingman.DI
{
    using Wingman.Container;

    internal class ArgumentBuilderFactory : IArgumentBuilderFactory
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        public ArgumentBuilderFactory(IDependencyRetriever dependencyRetriever)
        {
            _dependencyRetriever = dependencyRetriever;
        }

        public IArgumentBuilder CreateBuilderFor(IConstructor constructor)
        {
            return new DiArgumentBuilder(_dependencyRetriever, constructor);
        }
    }
}