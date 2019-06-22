namespace Wingman.DI
{
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class ObjectBuilderFactory : IObjectBuilderFactory
    {
        public IObjectBuilder CreateBuilder(IConstructorInvoker constructorInvoker, IArgumentBuilder argumentBuilder)
        {
            return new ObjectBuilder(constructorInvoker, argumentBuilder);
        }
    }
}