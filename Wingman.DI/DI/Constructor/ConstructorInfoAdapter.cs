namespace Wingman.DI.Constructor
{
    using System.Reflection;

    internal class ConstructorInfoAdapter : IConstructorInfo
    {
        private readonly ConstructorInfo _constructorInfo;

        public ConstructorInfoAdapter(ConstructorInfo constructorInfo)
        {
            _constructorInfo = constructorInfo;
        }

        public ParameterInfo[] GetParameters()
        {
            return _constructorInfo.GetParameters();
        }

        public object Invoke(object[] arguments)
        {
            return _constructorInfo.Invoke(arguments);
        }
    }
}