namespace Wingman.DI.Constructor
{
    using System;

    internal interface IDiConstructorMapFactory
    {
        IDiConstructorMap CreateConstructorMap(Type implementation);
    }
}