namespace Wingman.DI.Constructor
{
    using System;

    internal interface IArgumentConstructorMapFactory
    {
        IArgumentConstructorMap CreateConstructorMap(Type concreteType);
    }
}