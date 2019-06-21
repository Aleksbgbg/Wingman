namespace Wingman.Container.Strategies
{
    using System;

    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class LocationStrategyFactory : ILocationStrategyFactory
    {
        private readonly IConstructorCandidateEvaluator _constructorCandidateEvaluator;

        private readonly IDiArgumentBuilderFactory _diArgumentBuilderFactory;

        private readonly IDependencyRetriever _dependencyRetriever;

        public LocationStrategyFactory(IConstructorCandidateEvaluator constructorCandidateEvaluator,
                                       IDiArgumentBuilderFactory diArgumentBuilderFactory,
                                       IDependencyRetriever dependencyRetriever
        )
        {
            _constructorCandidateEvaluator = constructorCandidateEvaluator;
            _diArgumentBuilderFactory = diArgumentBuilderFactory;
            _dependencyRetriever = dependencyRetriever;
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
            return new DiStrategy(_constructorCandidateEvaluator, _diArgumentBuilderFactory, implementation);
        }
    }
}