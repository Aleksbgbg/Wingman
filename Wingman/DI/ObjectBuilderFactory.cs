namespace Wingman.DI
{
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class ObjectBuilderFactory : IObjectBuilderFactory
    {
        public IObjectBuilder CreateBuilder(IConstructor constructor, IArgumentBuilder argumentBuilder)
        {
            return new ObjectBuilder(constructor, argumentBuilder);
        }
    }
}