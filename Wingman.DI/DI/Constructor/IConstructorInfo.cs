namespace Wingman.DI.Constructor
{
    using System.Reflection;

    internal interface IConstructorInfo
    {
        ParameterInfo[] GetParameters();

        object Invoke(object[] arguments);
    }
}