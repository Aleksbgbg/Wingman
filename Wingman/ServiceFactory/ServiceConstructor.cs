namespace Wingman.ServiceFactory
{
    using System;
    using System.Reflection;

    internal class ServiceConstructor : IServiceConstructor
    {
        private readonly ConstructorInfo _info;

        private readonly ParameterInfo[] _parameters;

        internal ServiceConstructor(ConstructorInfo constructorInfo, object[] userArguments)
        {
            _info = constructorInfo;
            _parameters = constructorInfo.GetParameters();
            UserArguments = userArguments;
        }

        public int DependencyCount => ArgumentCount - UserArguments.Length;

        public int ArgumentCount => _parameters.Length;

        public object[] UserArguments { get; }

        public bool AcceptsUserArguments()
        {
            if (_parameters.Length < UserArguments.Length)
            {
                return false;
            }

            int lastUserArgumentIndex = UserArguments.Length - 1;
            int lastParameterIndex = _parameters.Length - 1;

            for (int argumentIndex = 0; argumentIndex < UserArguments.Length; ++argumentIndex)
            {
                object argument = UserArguments[lastUserArgumentIndex - argumentIndex];
                ParameterInfo parameter = _parameters[lastParameterIndex - argumentIndex];

                if (!parameter.ParameterType.IsInstanceOfType(argument))
                {
                    return false;
                }
            }

            return true;
        }

        public bool HasDependencies()
        {
            return _parameters.Length != UserArguments.Length;
        }

        public Type ArgumentTypeAtIndex(int index)
        {
            return _parameters[index].ParameterType;
        }

        public object CreateObjectUsingArguments(object[] arguments)
        {
            return _info.Invoke(arguments);
        }
    }
}