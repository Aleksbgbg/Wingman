namespace Wingman.Container.Strategies
{
    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class DiStrategy : IDiStrategy
    {
        private readonly IObjectBuilder _objectBuilder;

        internal DiStrategy(IDiConstructorMap diConstructorMap, IDiArgumentBuilderFactory diArgumentBuilderFactory, IObjectBuilderFactory objectBuilderFactory)
        {
            IConstructor constructor = diConstructorMap.FindBestConstructorForDi();
            IArgumentBuilder argumentBuilder = diArgumentBuilderFactory.CreateBuilderFor(constructor);

            _objectBuilder = objectBuilderFactory.CreateBuilder(constructor, argumentBuilder);
        }

        public object LocateService()
        {
            return _objectBuilder.BuildObject();
        }
    }
}