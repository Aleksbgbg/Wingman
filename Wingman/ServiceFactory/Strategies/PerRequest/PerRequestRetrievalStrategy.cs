namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    using System;

    using Wingman.DI;

    internal class PerRequestRetrievalStrategy : IServiceRetrievalStrategy
    {
        private readonly IUserArgumentBuilderFactory _userArgumentBuilderFactory;

        private readonly IConstructorMap _constructorMap;

        public PerRequestRetrievalStrategy(IUserArgumentBuilderFactory userArgumentBuilderFactory, IConstructorMapFactory constructorMapFactory, Type concreteType)
        {
            _userArgumentBuilderFactory = userArgumentBuilderFactory;
            _constructorMap = constructorMapFactory.CreateConstructorMap(concreteType);
        }

        public object RetrieveService(object[] arguments)
        {
            IConstructor constructor = _constructorMap.FindBestFitForArguments(arguments);

            object[] resolvedArguments = _userArgumentBuilderFactory.CreateBuilderFor(constructor, arguments)
                                                                    .BuildArguments();

            return constructor.Build(resolvedArguments);
        }
    }
}