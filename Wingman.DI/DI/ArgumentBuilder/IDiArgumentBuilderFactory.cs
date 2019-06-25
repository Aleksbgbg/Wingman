namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.DI.Constructor;

    internal interface IDiArgumentBuilderFactory
    {
        IArgumentBuilder CreateBuilderFor(IConstructorParameterInfo constructorParameterInfo);
    }
}