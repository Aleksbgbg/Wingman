namespace Wingman.ServiceFactory.Strategies
{
    using System;

    using Wingman.Container;
    using Wingman.ServiceFactory.Strategies.FromRetriever;
    using Wingman.ServiceFactory.Strategies.PerRequest;

    internal class RetrievalStrategyFactory : IRetrievalStrategyFactory
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        private readonly IArgumentBuilder _argumentBuilder;

        private readonly IConstructorMapFactory _constructorMapFactory;

        public RetrievalStrategyFactory(IDependencyRetriever dependencyRetriever, IArgumentBuilder argumentBuilder, IConstructorMapFactory constructorMapFactory)
        {
            _dependencyRetriever = dependencyRetriever;
            _argumentBuilder = argumentBuilder;
            _constructorMapFactory = constructorMapFactory;
        }

        public IServiceRetrievalStrategy FromRetriever(Type interfaceType)
        {
            return new FromRetrieverRetrievalStrategy(_dependencyRetriever, interfaceType);
        }

        public IServiceRetrievalStrategy PerRequest(Type concreteType)
        {
            return new PerRequestRetrievalStrategy(_argumentBuilder, _constructorMapFactory, concreteType);
        }
    }
}