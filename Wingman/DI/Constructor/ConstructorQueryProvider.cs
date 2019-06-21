namespace Wingman.DI.Constructor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class ConstructorQueryProvider : IConstructorQueryProvider
    {
        private readonly IConstructorFactory _constructorFactory;

        public ConstructorQueryProvider(IConstructorFactory constructorFactory)
        {
            _constructorFactory = constructorFactory;
        }

        public IEnumerable<IConstructor> QueryPublicInstanceConstructors(Type type)
        {
            return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                       .Select(_constructorFactory.CreateConstructor);
        }
    }
}