namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    using System;

    internal class PerRequestRetrievalStrategy : IServiceRetrievalStrategy
    {
        private readonly IArgumentBuilderFactory _argumentBuilderFactory;

        private readonly IConstructorMap _constructorMap;

        public PerRequestRetrievalStrategy(IArgumentBuilderFactory argumentBuilderFactory, IConstructorMapFactory constructorMapFactory, Type concreteType)
        {
            _argumentBuilderFactory = argumentBuilderFactory;
            _constructorMap = constructorMapFactory.MapConstructors(concreteType);
        }

        public object RetrieveService(object[] arguments)
        {
            IConstructor constructor = _constructorMap.FindBestFitForArguments(arguments);

            object[] resolvedArguments = _argumentBuilderFactory.CreateBuilderFor(constructor, arguments)
                                                                .BuildArguments();

            return constructor.Build(resolvedArguments);
        }
    }
}