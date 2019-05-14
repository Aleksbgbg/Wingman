namespace Wingman.ServiceFactory
{
    using System;

    using Wingman.Container;

    internal class TypeFactory
    {
        private readonly IDependencyContainer _dependencyContainer;

        private readonly IServiceConstructor _serviceConstructor;

        private object[] _constructorArguments;

        internal TypeFactory(IDependencyContainer dependencyContainer, IServiceConstructor serviceConstructor)
        {
            _dependencyContainer = dependencyContainer;
            _serviceConstructor = serviceConstructor;
        }

        internal object MakeType()
        {
            FillConstructorArguments();

            return _serviceConstructor.CreateObjectUsingArguments(_constructorArguments);
        }

        private void FillConstructorArguments()
        {
            if (_serviceConstructor.HasDependencies())
            {
                _constructorArguments = new object[_serviceConstructor.ArgumentCount];
                FillDependencyAndUserArguments();
            }
            else
            {
                _constructorArguments = _serviceConstructor.UserArguments;
            }
        }

        private void FillDependencyAndUserArguments()
        {
            int dependencyCount = _serviceConstructor.DependencyCount;

            FillDependencyArguments(dependencyCount);
            FillUserArgumentsStartingAt(dependencyCount);
        }

        private void FillDependencyArguments(int dependencyCount)
        {
            for (int argumentIndex = 0; argumentIndex < dependencyCount; ++argumentIndex)
            {
                Type dependencyType = _serviceConstructor.ArgumentTypeAtIndex(argumentIndex);
                object dependency = _dependencyContainer.GetInstance(dependencyType, key: null);

                _constructorArguments[argumentIndex] = dependency;
            }
        }

        private void FillUserArgumentsStartingAt(int startingIndex)
        {
            for (int argumentIndex = startingIndex; argumentIndex < _constructorArguments.Length; ++argumentIndex)
            {
                object argument = _serviceConstructor.UserArguments[argumentIndex - startingIndex];

                _constructorArguments[argumentIndex] = argument;
            }
        }
    }
}