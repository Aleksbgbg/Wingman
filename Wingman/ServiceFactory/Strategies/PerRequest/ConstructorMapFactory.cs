namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    using System;

    internal class ConstructorMapFactory : IConstructorMapFactory
    {
        private readonly IConstructorFactory _constructorFactory;

        public ConstructorMapFactory(IConstructorFactory constructorFactory)
        {
            _constructorFactory = constructorFactory;
        }

        public IConstructorMap MapConstructors(Type concreteType)
        {
            return new ConstructorMap(_constructorFactory, concreteType);
        }
    }
}