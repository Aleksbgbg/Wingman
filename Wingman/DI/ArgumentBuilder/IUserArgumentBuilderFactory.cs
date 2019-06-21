namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.DI.Constructor;

    internal interface IUserArgumentBuilderFactory
    {
        IArgumentBuilder CreateBuilderFor(IConstructor constructor, object[] userArguments);
    }
}