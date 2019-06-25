namespace Wingman.DI.Constructor
{
    using System;
    using System.Linq;

    using Wingman.Utilities.ThrowHelper;

    internal class ConstructorMap : IArgumentConstructorMap, IDiConstructorMap
    {
        private readonly IConstructor[] _constructors;

        internal ConstructorMap(IConstructorQueryProvider constructorQueryProvider, Type concreteType)
        {
            _constructors = constructorQueryProvider.QueryPublicInstanceConstructors(concreteType)
                                                    .ToArray();

            if (_constructors.Length == 0)
            {
                throw ThrowHelper.ConstructorMap.NoPublicInstanceConstructors(concreteType);
            }
        }

        public IConstructionInfo FindBestConstructorForArguments(object[] arguments)
        {
            return _constructors.Single(constructor => constructor.AcceptsUserArguments(arguments));
        }

        public IConstructionInfo FindBestConstructorForDi()
        {
            return _constructors.OrderBy(constructor => constructor.ParameterCount)
                                .First();
        }
    }
}