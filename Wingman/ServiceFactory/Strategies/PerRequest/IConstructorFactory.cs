namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    using System.Reflection;

    internal interface IConstructorFactory
    {
        IConstructor CreateConstructor(ConstructorInfo constructorInfo);
    }
}