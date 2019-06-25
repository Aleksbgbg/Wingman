namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.DI.Constructor;

    internal interface IUserArgumentBuilderFactory
    {
        IArgumentBuilder CreateBuilderFor(IConstructorParameterInfo constructorParameterInfo, object[] userArguments);
    }
}