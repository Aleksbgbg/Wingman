namespace Wingman.ServiceFactory
{
    using System;

    internal interface IConstructorMapFactory
    {
        IConstructorMap MapConstructors(Type concreteType);
    }
}