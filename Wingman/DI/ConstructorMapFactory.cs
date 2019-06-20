namespace Wingman.DI
{
    using System;

    internal class ConstructorMapFactory : IConstructorMapFactory
    {
        private readonly IConstructorFactory _constructorFactory;

        public ConstructorMapFactory(IConstructorFactory constructorFactory)
        {
            _constructorFactory = constructorFactory;
        }

        public IConstructorMap CreateConstructorMap(Type concreteType)
        {
            return new ConstructorMap(_constructorFactory, concreteType);
        }
    }
}