namespace Wingman.DI.Constructor
{
    using System;
    using System.Linq;

    using Wingman.Utilities;

    internal class ConstructorMap : IArgumentConstructorMap, IDiConstructorMap
    {
        private readonly IConstructor[] _constructors;

        internal ConstructorMap(IConstructorQueryProvider constructorQueryProvider, Type concreteType)
        {
            _constructors = constructorQueryProvider.QueryPublicInstanceConstructors(concreteType)
                                                    .ToArray();

            if (_constructors.Length == 0)
            {
                ThrowHelper.Throw.ConstructorMap.NoPublicInstanceConstructors(concreteType);
            }
        }

        public IConstructor FindBestConstructorForArguments(object[] arguments)
        {
            return _constructors.Single(constructor => constructor.AcceptsUserArguments(arguments));
        }

        public IConstructor FindBestConstructorForDi()
        {
            return _constructors.OrderBy(constructor => constructor.ParameterCount)
                                .First();
        }
    }
}