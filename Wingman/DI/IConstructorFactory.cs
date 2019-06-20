namespace Wingman.DI
{
    using System.Reflection;

    internal interface IConstructorFactory
    {
        IConstructor CreateConstructor(ConstructorInfo constructorInfo);
    }
}