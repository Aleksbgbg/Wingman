namespace Wingman.ServiceFactory
{
    using System;

    internal class PerRequestRetrievalStrategy : IServiceRetrievalStrategy
    {
        private readonly IArgumentBuilder _argumentBuilder;

        private readonly IConstructorMap _constructorMap;

        public PerRequestRetrievalStrategy(IArgumentBuilder argumentBuilder, IConstructorMapFactory constructorMapFactory, Type concreteType)
        {
            _argumentBuilder = argumentBuilder;
            _constructorMap = constructorMapFactory.MapConstructors(concreteType);
        }

        public object RetrieveService(object[] arguments)
        {
            IConstructor constructor = _constructorMap.FindBestFitForArguments(arguments);

            object[] resolvedArguments = _argumentBuilder.BuildArgumentsForConstructor(constructor, arguments);

            return constructor.Build(resolvedArguments);
        }
    }
}