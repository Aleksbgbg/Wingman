namespace Wingman.Container.Strategies
{
    using System;

    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    internal class DiStrategy : IDiStrategy
    {
        private readonly IConstructor _targetConstructor;

        private readonly IArgumentBuilder _argumentBuilder;

        internal DiStrategy(IConstructorCandidateEvaluator constructorCandidateEvaluator, IDiArgumentBuilderFactory diArgumentBuilderFactory, Type implementation)
        {
            _targetConstructor = constructorCandidateEvaluator.FindBestConstructorForDi(implementation);
            _argumentBuilder = diArgumentBuilderFactory.CreateBuilderFor(_targetConstructor);
        }

        public object LocateService()
        {
            object[] arguments = _argumentBuilder.BuildArguments();

            return _targetConstructor.Build(arguments);
        }
    }
}