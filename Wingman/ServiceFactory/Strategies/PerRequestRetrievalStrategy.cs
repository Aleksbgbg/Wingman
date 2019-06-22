namespace Wingman.ServiceFactory.Strategies
{
    using System;

    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class PerRequestRetrievalStrategy : IServiceRetrievalStrategy
    {
        private readonly IConstructorMap _constructorMap;

        private readonly IUserArgumentBuilderFactory _userArgumentBuilderFactory;

        private readonly IObjectBuilderFactory _objectBuilderFactory;

        public PerRequestRetrievalStrategy(IConstructorMapFactory constructorMapFactory,
                                           IUserArgumentBuilderFactory userArgumentBuilderFactory,
                                           IObjectBuilderFactory objectBuilderFactory,
                                           Type concreteType)
        {
            _constructorMap = constructorMapFactory.CreateConstructorMap(concreteType);
            _userArgumentBuilderFactory = userArgumentBuilderFactory;
            _objectBuilderFactory = objectBuilderFactory;
        }

        public object RetrieveService(object[] arguments)
        {
            IConstructor constructor = _constructorMap.FindBestFitForArguments(arguments);
            IArgumentBuilder argumentBuilder = _userArgumentBuilderFactory.CreateBuilderFor(constructor, arguments);

            IObjectBuilder objectBuilder = _objectBuilderFactory.CreateBuilder(constructor, argumentBuilder);

            return objectBuilder.BuildObject();
        }
    }
}