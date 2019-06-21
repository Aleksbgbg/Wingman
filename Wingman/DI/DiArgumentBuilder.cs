namespace Wingman.DI
{
    using Wingman.Container;

    internal class DiArgumentBuilder : ArgumentBuilderBase
    {
        private readonly IConstructor _constructor;

        internal DiArgumentBuilder(IDependencyRetriever dependencyRetriever, IConstructor constructor) : base(dependencyRetriever, constructor)
        {
            _constructor = constructor;
        }

        private protected override void InstantiateAndFillArguments()
        {
            Arguments = new object[_constructor.ParameterCount];
            ResolveDependenciesFromStart(Arguments.Length);
        }
    }
}