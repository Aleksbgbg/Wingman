namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Wingman.Utilities;

    internal class ConstructorMap : IConstructorMap
    {
        private readonly IConstructor[] _constructors;

        internal ConstructorMap(IConstructorFactory constructorFactory, Type concreteType)
        {
            _constructors = concreteType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                                        .Select(constructorFactory.MakeConstructor)
                                        .ToArray();

            if (_constructors.Length == 0)
            {
                ThrowHelper.Throw.ConstructorMap.NoPublicInstanceConstructors(concreteType);
            }
        }

        public IConstructor FindBestFitForArguments(object[] arguments)
        {
            return _constructors.Single(constructor => constructor.AcceptsUserArguments(arguments));
        }
    }
}