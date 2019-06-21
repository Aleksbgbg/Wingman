namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.Container;
    using Wingman.DI.Constructor;

    internal class UserArgumentBuilderFactory : IUserArgumentBuilderFactory
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        public UserArgumentBuilderFactory(IDependencyRetriever dependencyRetriever)
        {
            _dependencyRetriever = dependencyRetriever;
        }

        public IArgumentBuilder CreateBuilderFor(IConstructor constructor, object[] userArguments)
        {
            return new UserArgumentBuilder(_dependencyRetriever, constructor, userArguments);
        }
    }
}