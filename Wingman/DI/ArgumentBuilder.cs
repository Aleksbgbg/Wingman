﻿namespace Wingman.DI
{
    using Wingman.Container;

    internal class ArgumentBuilder : IArgumentBuilder
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        private readonly IConstructor _constructor;

        private readonly object[] _userArguments;

        private object[] _arguments;

        public ArgumentBuilder(IDependencyRetriever dependencyRetriever, IConstructor constructor, object[] userArguments)
        {
            _dependencyRetriever = dependencyRetriever;
            _constructor = constructor;
            _userArguments = userArguments;
        }

        public object[] BuildArguments()
        {
            if (UserArgumentsFitConstructor())
            {
                _arguments = _userArguments;
            }
            else
            {
                _arguments = new object[_constructor.ParameterCount];
                FillArguments();
            }

            return _arguments;
        }

        private bool UserArgumentsFitConstructor()
        {
            return _userArguments.Length == _constructor.ParameterCount;
        }

        private void FillArguments()
        {
            int dependencyCount = _constructor.ParameterCount - _userArguments.Length;

            ResolveDependencies(dependencyCount);
            FillUserArguments(dependencyCount);
        }

        private void ResolveDependencies(int dependencyCount)
        {
            for (int dependencyIndex = 0; dependencyIndex < dependencyCount; ++dependencyIndex)
            {
                _arguments[dependencyIndex] = _dependencyRetriever.GetInstance(_constructor.ParameterTypeAt(dependencyIndex));
            }
        }

        private void FillUserArguments(int startingIndex)
        {
            for (int userArgumentIndex = 0; userArgumentIndex < _userArguments.Length; ++userArgumentIndex)
            {
                _arguments[startingIndex + userArgumentIndex] = _userArguments[userArgumentIndex];
            }
        }
    }
}