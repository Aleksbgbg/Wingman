namespace Wingman.DI
{
    using System.Reflection;

    internal interface IConstructorInfo
    {
        ParameterInfo[] GetParameters();

        object Invoke(object[] arguments);
    }
}