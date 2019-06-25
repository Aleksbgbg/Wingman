namespace Wingman.DI.Constructor
{
    using System;
    using System.Collections.Generic;

    internal interface IConstructorQueryProvider
    {
        IEnumerable<IConstructor> QueryPublicInstanceConstructors(Type type);
    }
}