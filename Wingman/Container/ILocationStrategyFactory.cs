﻿namespace Wingman.Container
{
    using System;

    internal interface ILocationStrategyFactory
    {
        IServiceLocationStrategy CreateInstance(object instance);

        IServiceLocationStrategy CreateSingleton(Type implementation);

        IServiceLocationStrategy CreatePerRequest(Type implementation);

        IServiceLocationStrategy CreateHandler(Func<IDependencyRetriever, object> handler);
    }
}