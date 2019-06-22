namespace Wingman.DI
{
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal interface IObjectBuilderFactory
    {
        IObjectBuilder CreateBuilder(IConstructor constructor, IArgumentBuilder argumentBuilder);
    }
}