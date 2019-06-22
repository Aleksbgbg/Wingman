namespace Wingman.DI
{
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class ObjectBuilder : IObjectBuilder
    {
        private readonly IConstructor _constructor;

        private readonly IArgumentBuilder _argumentBuilder;

        public ObjectBuilder(IConstructor constructor, IArgumentBuilder argumentBuilder)
        {
            _constructor = constructor;
            _argumentBuilder = argumentBuilder;
        }

        public object Build()
        {
            object[] arguments = _argumentBuilder.BuildArguments();
            return _constructor.Build(arguments);
        }
    }
}