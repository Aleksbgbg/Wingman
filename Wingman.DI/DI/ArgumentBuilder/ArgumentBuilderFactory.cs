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

        public IArgumentBuilder CreateBuilderFor(IConstructorParameterInfo constructorParameterInfo)
        {
            return new DiArgumentBuilder(_dependencyRetriever, constructorParameterInfo);
        }

        public IArgumentBuilder CreateBuilderFor(IConstructorParameterInfo constructorParameterInfo, object[] userArguments)
        {
            return new UserArgumentBuilder(_dependencyRetriever, constructorParameterInfo, userArguments);
        }
    }
}