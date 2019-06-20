namespace Wingman.DI
{
    internal interface IArgumentBuilderFactory
    {
        IArgumentBuilder CreateBuilderFor(IConstructor constructor);
    }
}