namespace Wingman.DI.ArgumentBuilder
{
    using Wingman.Container;
    using Wingman.DI.Constructor;

    internal class UserArgumentBuilder : ArgumentBuilderBase
    {
        private readonly IConstructor _constructor;

        private readonly object[] _userArguments;

        public UserArgumentBuilder(IDependencyRetriever dependencyRetriever, IConstructor constructor, object[] userArguments) : base(dependencyRetriever, constructor)
        {
            _constructor = constructor;
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
                Arguments = new object[_constructor.ParameterCount];
                FillArguments();
            }
        }

        private bool UserArgumentsFitConstructor()
        {
            return _userArguments.Length == _constructor.ParameterCount;
        }

        private void FillArguments()
        {
            int dependencyCount = _constructor.ParameterCount - _userArguments.Length;

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