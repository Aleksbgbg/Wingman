namespace Wingman.Container
{
    using System;

    public class HandlerStrategy : IServiceLocationStrategy
    {
        private readonly Func<IDependencyRetriever, object> _handler;

        public HandlerStrategy(Func<IDependencyRetriever, object> handler)
        {
            _handler = handler;
        }

        public object LocateService(IDependencyRetriever dependencyRetriever)
        {
            return _handler(dependencyRetriever);
        }
    }
}