namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.Container;
    using Wingman.DI.Constructor;

    internal class UserArgumentBuilder : ArgumentBuilderBase
    {
        private readonly IConstructorParameterInfo _constructorParameterInfo;

        private readonly object[] _userArguments;

        public UserArgumentBuilder(IDependencyRetriever dependencyRetriever, IConstructorParameterInfo constructorParameterInfo, object[] userArguments) : base(dependencyRetriever, constructorParameterInfo)
        {
            _constructorParameterInfo = constructorParameterInfo;
            _userArguments = userArguments;
        }

        private protected override void InstantiateAndFillArguments()
        {
            if (UserArgumentsFitConstructor())
            {
                Arguments = _userArguments;
            }
            else
            {
                Arguments = new object[_constructorParameterInfo.ParameterCount];
                FillArguments();
            }
        }

        private bool UserArgumentsFitConstructor()
        {
            return _userArguments.Length == _constructorParameterInfo.ParameterCount;
        }

        private void FillArguments()
        {
            int dependencyCount = _constructorParameterInfo.ParameterCount - _userArguments.Length;

            ResolveDependenciesFromStart(dependencyCount);
            FillUserArguments(dependencyCount);
        }

        private void FillUserArguments(int startingIndex)
        {
            for (int userArgumentIndex = 0; userArgumentIndex < _userArguments.Length; ++userArgumentIndex)
            {
                Arguments[startingIndex + userArgumentIndex] = _userArguments[userArgumentIndex];
            }
        }
    }
}