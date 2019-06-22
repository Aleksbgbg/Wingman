namespace Wingman.DI
{
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class ObjectBuilderFactory : IObjectBuilderFactory
    {
        public IObjectBuilder CreateBuilder(IConstructorBuilder constructorBuilder, IArgumentBuilder argumentBuilder)
        {
            return new ObjectBuilder(constructorBuilder, argumentBuilder);
        }
    }
}