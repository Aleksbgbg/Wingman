namespace Wingman.ServiceFactory.Strategies
{
    using System;

    using Wingman.Container;
    using Wingman.ServiceFactory.Strategies.FromRetriever;
    using Wingman.ServiceFactory.Strategies.PerRequest;

    internal class RetrievalStrategyFactory : IRetrievalStrategyFactory
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        private readonly IArgumentBuilderFactory _argumentBuilderFactory;

        private readonly IConstructorMapFactory _constructorMapFactory;

        public RetrievalStrategyFactory(IDependencyRetriever dependencyRetriever, IArgumentBuilderFactory argumentBuilderFactory, IConstructorMapFactory constructorMapFactory)
        {
            _dependencyRetriever = dependencyRetriever;
            _argumentBuilderFactory = argumentBuilderFactory;
            _constructorMapFactory = constructorMapFactory;
        }

        public IServiceRetrievalStrategy CreateFromRetriever(Type interfaceType)
        {
            return new FromRetrieverRetrievalStrategy(_dependencyRetriever, interfaceType);
        }

        public IServiceRetrievalStrategy CreatePerRequest(Type concreteType)
        {
            return new PerRequestRetrievalStrategy(_argumentBuilderFactory, _constructorMapFactory, concreteType);
        }
    }
}