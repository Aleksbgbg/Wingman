namespace Wingman.ServiceFactory.Strategies
{
    using System;

    using Wingman.Container;
    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class RetrievalStrategyFactory : IRetrievalStrategyFactory
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        private readonly IConstructorMapFactory _constructorMapFactory;

        private readonly IUserArgumentBuilderFactory _userArgumentBuilderFactory;

        private readonly IObjectBuilderFactory _objectBuilderFactory;

        public RetrievalStrategyFactory(IDependencyRetriever dependencyRetriever,
                                        IConstructorMapFactory constructorMapFactory,
                                        IUserArgumentBuilderFactory userArgumentBuilderFactory,
                                        IObjectBuilderFactory objectBuilderFactory)
        {
            _dependencyRetriever = dependencyRetriever;
            _constructorMapFactory = constructorMapFactory;
            _userArgumentBuilderFactory = userArgumentBuilderFactory;
            _objectBuilderFactory = objectBuilderFactory;
        }

        public IServiceRetrievalStrategy CreateFromRetriever(Type interfaceType)
        {
            return new FromRetrieverRetrievalStrategy(_dependencyRetriever, interfaceType);
        }

        public IServiceRetrievalStrategy CreatePerRequest(Type concreteType)
        {
            return new PerRequestRetrievalStrategy(_constructorMapFactory, _userArgumentBuilderFactory, _objectBuilderFactory, concreteType);
        }
    }
}