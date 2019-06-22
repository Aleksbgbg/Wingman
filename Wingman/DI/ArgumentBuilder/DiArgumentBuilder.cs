namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.Container;
    using Wingman.DI.Constructor;

    internal class DiArgumentBuilder : ArgumentBuilderBase
    {
        private readonly IConstructorParameterInfo _constructorParameterInfo;

        internal DiArgumentBuilder(IDependencyRetriever dependencyRetriever, IConstructorParameterInfo constructorParameterInfo) : base(dependencyRetriever, constructorParameterInfo)
        {
            _constructorParameterInfo = constructorParameterInfo;
        }

        private protected override void InstantiateAndFillArguments()
        {
            Arguments = new object[_constructorParameterInfo.ParameterCount];
            ResolveDependenciesFromStart(Arguments.Length);
        }
    }
}