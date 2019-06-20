namespace Wingman.Container.Strategies
{
    using System;

    using Wingman.DI;

    internal class DiStrategy : IDiStrategy
    {
        private readonly IConstructor _targetConstructor;

        private readonly IArgumentBuilder _argumentBuilder;

        internal DiStrategy(IConstructorCandidateEvaluator constructorCandidateEvaluator, IArgumentBuilderFactory argumentBuilderFactory, Type implementation)
        {
            _targetConstructor = constructorCandidateEvaluator.FindBestConstructorForDi(implementation);
            _argumentBuilder = argumentBuilderFactory.CreateBuilderFor(_targetConstructor);
        }

        public object LocateService()
        {
            object[] arguments = _argumentBuilder.BuildArguments();

            return _targetConstructor.Build(arguments);
        }
    }
}