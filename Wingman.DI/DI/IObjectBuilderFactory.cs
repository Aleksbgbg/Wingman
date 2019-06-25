namespace Wingman.DI
{
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal interface IObjectBuilderFactory
    {
        IObjectBuilder CreateBuilder(IConstructorInvoker constructorInvoker, IArgumentBuilder argumentBuilder);
    }
}