namespace Wingman.Container.Strategies
{
    using System;

    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class LocationStrategyFactory : ILocationStrategyFactory
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        private readonly IDiConstructorMapFactory _diConstructorMapFactory;

        private readonly IDiArgumentBuilderFactory _diArgumentBuilderFactory;

        private readonly IObjectBuilderFactory _objectBuilderFactory;

        public LocationStrategyFactory(IDependencyRetriever dependencyRetriever,
                                       IDiConstructorMapFactory diConstructorMapFactory,
                                       IDiArgumentBuilderFactory diArgumentBuilderFactory,
                                       IObjectBuilderFactory objectBuilderFactory
        )
        {
            _dependencyRetriever = dependencyRetriever;
            _diConstructorMapFactory = diConstructorMapFactory;
            _diArgumentBuilderFactory = diArgumentBuilderFactory;
            _objectBuilderFactory = objectBuilderFactory;
        }

        public IServiceLocationStrategy CreateInstance(object implementation)
        {
            return new InstanceStrategy(implementation);
        }

        public IServiceLocationStrategy CreateSingleton(Type implementation)
        {
            return new SingletonStrategy(CreateDiStrategy(implementation));
        }

        public IServiceLocationStrategy CreatePerRequest(Type implementation)
        {
            return new PerRequestStrategy(CreateDiStrategy(implementation));
        }

        public IServiceLocationStrategy CreateHandler(Func<IDependencyRetriever, object> handler)
        {
            return new HandlerStrategy(_dependencyRetriever, handler);
        }

        private DiStrategy CreateDiStrategy(Type implementation)
        {
            return new DiStrategy(_diConstructorMapFactory.CreateConstructorMap(implementation), _diArgumentBuilderFactory, _objectBuilderFactory);
        }
    }
}