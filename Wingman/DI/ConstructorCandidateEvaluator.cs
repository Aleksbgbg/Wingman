namespace Wingman.DI
{
    using System;
    using System.Linq;

    using Wingman.Utilities;

    internal class ConstructorCandidateEvaluator : IConstructorCandidateEvaluator
    {
        private readonly IConstructorQueryProvider _constructorQueryProvider;

        public ConstructorCandidateEvaluator(IConstructorQueryProvider constructorQueryProvider)
        {
            _constructorQueryProvider = constructorQueryProvider;
        }

        public IConstructor FindBestConstructorForDi(Type implementation)
        {
            IConstructor matchingConstructor =  _constructorQueryProvider.QueryPublicInstanceConstructors(implementation)
                                                                         .OrderBy(constructor => constructor.ParameterCount)
                                                                         .FirstOrDefault();

            if (matchingConstructor == null)
            {
                ThrowHelper.Throw.ConstructorCandidateEvaluator.NoPublicInstanceConstructors(implementation);
            }

            return matchingConstructor;
        }
    }
}