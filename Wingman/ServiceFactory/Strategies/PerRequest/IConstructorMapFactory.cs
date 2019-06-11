namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    using System;

    internal interface IConstructorMapFactory
    {
        IConstructorMap CreateConstructorMap(Type concreteType);
    }
}