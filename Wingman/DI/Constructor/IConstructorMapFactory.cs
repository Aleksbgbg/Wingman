namespace Wingman.DI.Constructor
{
    using System;

    internal interface IConstructorMapFactory
    {
        IConstructorMap CreateConstructorMap(Type concreteType);
    }
}