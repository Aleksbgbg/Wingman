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

        public IArgumentBuilder CreateBuilderFor(IConstructor constructor, object[] userArguments)
        {
            return new ArgumentBuilder(_dependencyRetriever, constructor, userArguments);
        }
    }
}