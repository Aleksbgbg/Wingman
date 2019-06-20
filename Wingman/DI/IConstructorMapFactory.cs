namespace Wingman.DI
{
    using System;

    internal interface IConstructorMapFactory
    {
        IConstructorMap CreateConstructorMap(Type concreteType);
    }
}