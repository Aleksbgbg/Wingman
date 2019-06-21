namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.Container;
    using Wingman.DI.Constructor;

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