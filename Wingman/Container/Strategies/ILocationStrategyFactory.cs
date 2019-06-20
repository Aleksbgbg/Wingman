namespace Wingman.Container.Strategies
{
    using System;

    internal interface ILocationStrategyFactory
    {
        IServiceLocationStrategy CreateInstance(object implementation);

        IServiceLocationStrategy CreateSingleton(Type implementation);

        IServiceLocationStrategy CreatePerRequest(Type implementation);

        IServiceLocationStrategy CreateHandler(Func<IDependencyRetriever, object> handler);
    }
}