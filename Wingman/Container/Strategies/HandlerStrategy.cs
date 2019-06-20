namespace Wingman.Container.Strategies
{
    using System;

    public class HandlerStrategy : IServiceLocationStrategy
    {
        private readonly IDependencyRetriever _dependencyRetriever;

        private readonly Func<IDependencyRetriever, object> _handler;

        public HandlerStrategy(IDependencyRetriever dependencyRetriever, Func<IDependencyRetriever, object> handler)
        {
            _dependencyRetriever = dependencyRetriever;
            _handler = handler;
        }

        public object LocateService()
        {
            return _handler(_dependencyRetriever);
        }
    }
}