namespace Wingman.DI
{
    using System;
    using System.Collections.Generic;

    internal interface IConstructorQueryProvider
    {
        IEnumerable<IConstructor> QueryPublicInstanceConstructors(Type type);
    }
}