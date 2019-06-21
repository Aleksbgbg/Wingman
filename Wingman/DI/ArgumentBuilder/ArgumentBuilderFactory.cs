namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.Container;
    using Wingman.DI.Constructor;

    internal class ArgumentBuilderFactory : IDiArgumentBuilderFactory, IUserArgumentBuilderFactory
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

        public IArgumentBuilder CreateBuilderFor(IConstructor constructor, object[] userArguments)
        {
            return new UserArgumentBuilder(_dependencyRetriever, constructor, userArguments);
        }
    }
}