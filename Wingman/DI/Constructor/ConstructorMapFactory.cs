namespace Wingman.DI.Constructor
{
    using System;

    internal class ConstructorMapFactory : IConstructorMapFactory
    {
        private readonly IConstructorQueryProvider _constructorQueryProvider;

        public ConstructorMapFactory(IConstructorQueryProvider constructorQueryProvider)
        {
            _constructorQueryProvider = constructorQueryProvider;
        }

        public IArgumentConstructorMap CreateConstructorMap(Type concreteType)
        {
            return new ConstructorMap(_constructorQueryProvider, concreteType);
        }
    }
}