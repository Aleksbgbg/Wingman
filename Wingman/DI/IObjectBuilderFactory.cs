namespace Wingman.DI
{
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal interface IObjectBuilderFactory
    {
        IObjectBuilder CreateBuilder(IConstructorBuilder constructorBuilder, IArgumentBuilder argumentBuilder);
    }
}