namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.DI.Constructor;

    internal interface IArgumentBuilderFactory
    {
        IArgumentBuilder CreateBuilderFor(IConstructor constructor);
    }
}