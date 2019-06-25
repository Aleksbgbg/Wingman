namespace Wingman.DI.Constructor
{
    using System.Reflection;

    internal interface IConstructorFactory
    {
        IConstructor CreateConstructor(ConstructorInfo constructorInfo);
    }
}