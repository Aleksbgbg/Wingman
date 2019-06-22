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

        private readonly IArgumentConstructorMapFactory _argumentConstructorMapFactory;

        private readonly IUserArgumentBuilderFactory _userArgumentBuilderFactory;

        private readonly IObjectBuilderFactory _objectBuilderFactory;

        public RetrievalStrategyFactory(IDependencyRetriever dependencyRetriever,
                                        IArgumentConstructorMapFactory argumentConstructorMapFactory,
                                        IUserArgumentBuilderFactory userArgumentBuilderFactory,
                                        IObjectBuilderFactory objectBuilderFactory)
        {
            _dependencyRetriever = dependencyRetriever;
            _argumentConstructorMapFactory = argumentConstructorMapFactory;
            _userArgumentBuilderFactory = userArgumentBuilderFactory;
            _objectBuilderFactory = objectBuilderFactory;
        }

        public IServiceRetrievalStrategy CreateFromRetriever(Type interfaceType)
        {
            return new FromRetrieverRetrievalStrategy(_dependencyRetriever, interfaceType);
        }

        public IServiceRetrievalStrategy CreatePerRequest(Type concreteType)
        {
            return new PerRequestRetrievalStrategy(_argumentConstructorMapFactory.CreateConstructorMap(concreteType), _userArgumentBuilderFactory, _objectBuilderFactory);
        }
    }
}