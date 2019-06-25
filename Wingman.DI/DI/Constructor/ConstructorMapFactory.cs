namespace Wingman.DI.Constructor
{
    using System;

    internal class ConstructorMapFactory : IArgumentConstructorMapFactory, IDiConstructorMapFactory
    {
        private readonly IConstructorQueryProvider _constructorQueryProvider;

        public ConstructorMapFactory(IConstructorQueryProvider constructorQueryProvider)
        {
            _constructorQueryProvider = constructorQueryProvider;
        }

        IArgumentConstructorMap IArgumentConstructorMapFactory.CreateConstructorMap(Type concreteType)
        {
            return CreateConstructorMap(concreteType);
        }

        IDiConstructorMap IDiConstructorMapFactory.CreateConstructorMap(Type implementation)
        {
            return CreateConstructorMap(implementation);
        }

        private ConstructorMap CreateConstructorMap(Type type)
        {
            return new ConstructorMap(_constructorQueryProvider, type);
        }
    }
}