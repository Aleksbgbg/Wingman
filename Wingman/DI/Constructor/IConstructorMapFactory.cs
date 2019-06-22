namespace Wingman.DI.Constructor
{
    using System;

    internal interface IConstructorMapFactory
    {
        IArgumentConstructorMap CreateConstructorMap(Type concreteType);
    }
}