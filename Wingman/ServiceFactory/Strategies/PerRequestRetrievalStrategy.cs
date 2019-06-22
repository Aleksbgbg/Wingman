namespace Wingman.ServiceFactory.Strategies
{
    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class PerRequestRetrievalStrategy : IServiceRetrievalStrategy
    {
        private readonly IArgumentConstructorMap _argumentConstructorMap;

        private readonly IUserArgumentBuilderFactory _userArgumentBuilderFactory;

        private readonly IObjectBuilderFactory _objectBuilderFactory;

        public PerRequestRetrievalStrategy(IArgumentConstructorMap argumentConstructorMap,
                                           IUserArgumentBuilderFactory userArgumentBuilderFactory,
                                           IObjectBuilderFactory objectBuilderFactory)
        {
            _argumentConstructorMap = argumentConstructorMap;
            _userArgumentBuilderFactory = userArgumentBuilderFactory;
            _objectBuilderFactory = objectBuilderFactory;
        }

        public object RetrieveService(object[] arguments)
        {
            IConstructor constructor = _argumentConstructorMap.FindBestConstructorForArguments(arguments);
            IArgumentBuilder argumentBuilder = _userArgumentBuilderFactory.CreateBuilderFor(constructor, arguments);

            IObjectBuilder objectBuilder = _objectBuilderFactory.CreateBuilder(constructor, argumentBuilder);

            return objectBuilder.BuildObject();
        }
    }
}