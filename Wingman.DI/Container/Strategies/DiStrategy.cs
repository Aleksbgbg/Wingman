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
            IConstructionInfo constructionInfo = diConstructorMap.FindBestConstructorForDi();
            IArgumentBuilder argumentBuilder = diArgumentBuilderFactory.CreateBuilderFor(constructionInfo);

            _objectBuilder = objectBuilderFactory.CreateBuilder(constructionInfo, argumentBuilder);
        }

        public object LocateService()
        {
            return _objectBuilder.BuildObject();
        }
    }
}