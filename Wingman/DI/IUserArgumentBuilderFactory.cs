namespace Wingman.DI
{
    internal interface IUserArgumentBuilderFactory
    {
        IArgumentBuilder CreateBuilderFor(IConstructor constructor, object[] userArguments);
    }
}