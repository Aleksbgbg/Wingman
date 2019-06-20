namespace Wingman.ServiceFactory.Strategies
{
    using System;

    using Wingman.Container;
    using Wingman.DI;
    using Wingman.ServiceFactory.Strategies.FromRetriever;
    using Wingman.ServiceFactory.Strategies.PerRequest;

    internal class RetrievalStrategyFactory : IRetrievalStrategyFactory
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        private readonly IUserArgumentBuilderFactory _userArgumentBuilderFactory;

        private readonly IConstructorMapFactory _constructorMapFactory;

        public RetrievalStrategyFactory(IDependencyRetriever dependencyRetriever, IUserArgumentBuilderFactory userArgumentBuilderFactory, IConstructorMapFactory constructorMapFactory)
        {
            _dependencyRetriever = dependencyRetriever;
            _userArgumentBuilderFactory = userArgumentBuilderFactory;
            _constructorMapFactory = constructorMapFactory;
        }

        public IServiceRetrievalStrategy CreateFromRetriever(Type interfaceType)
        {
            return new FromRetrieverRetrievalStrategy(_dependencyRetriever, interfaceType);
        }

        public IServiceRetrievalStrategy CreatePerRequest(Type concreteType)
        {
            return new PerRequestRetrievalStrategy(_userArgumentBuilderFactory, _constructorMapFactory, concreteType);
        }
    }
}