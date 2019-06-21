namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.Container;
    using Wingman.DI.Constructor;

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