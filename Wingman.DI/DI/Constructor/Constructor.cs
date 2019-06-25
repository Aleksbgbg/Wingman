namespace Wingman.DI.Constructor
{
    using System;
    using System.Reflection;

    internal class Constructor : IConstructor
    {
        private readonly IConstructorInfo _constructorInfo;

        private readonly ParameterInfo[] _parameters;

        public Constructor(IConstructorInfo constructorInfo)
        {
            _constructorInfo = constructorInfo;
            _parameters = constructorInfo.GetParameters();
        }

        public int ParameterCount => _parameters.Length;

        public Type ParameterTypeAt(int index)
        {
            return _parameters[index].ParameterType;
        }

        public bool AcceptsUserArguments(object[] userArguments)
        {
            return ArgumentsFitIntoParameters(userArguments) && ParameterTypesMatchFromEnd(userArguments);
        }

        public object InvokeConstructor(object[] arguments)
        {
            return _constructorInfo.Invoke(arguments);
        }

        private bool ArgumentsFitIntoParameters(object[] userArguments)
        {
            return userArguments.Length <= ParameterCount;
        }

        private bool ParameterTypesMatchFromEnd(object[] userArguments)
        {
            int parameterOffset = ParameterCount - userArguments.Length;

            for (int index = 0; index < userArguments.Length; index++)
            {
                Type parameterType = _parameters[parameterOffset + index].ParameterType;
                Type argumentType = userArguments[index].GetType();

                if (parameterType != argumentType)
                {
                    return false;
                }
            }

            return true;
        }
    }
}