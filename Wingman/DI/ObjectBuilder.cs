namespace Wingman.DI
{
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class ObjectBuilder : IObjectBuilder
    {
        private readonly IConstructorBuilder _constructor;

        private readonly IArgumentBuilder _argumentBuilder;

        public ObjectBuilder(IConstructorBuilder constructor, IArgumentBuilder argumentBuilder)
        {
            _constructor = constructor;
            _argumentBuilder = argumentBuilder;
        }

        public object BuildObject()
        {
            object[] arguments = _argumentBuilder.BuildArguments();
            return _constructor.BuildWith(arguments);
        }
    }
}