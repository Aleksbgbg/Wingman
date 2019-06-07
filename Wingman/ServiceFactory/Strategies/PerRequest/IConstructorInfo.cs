namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    using System.Reflection;

    internal interface IConstructorInfo
    {
        ParameterInfo[] GetParameters();

        object Invoke(object[] arguments);
    }
}