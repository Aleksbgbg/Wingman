namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.DI.Constructor;

    internal interface IDiArgumentBuilderFactory
    {
        IArgumentBuilder CreateBuilderFor(IConstructor constructor);
    }
}