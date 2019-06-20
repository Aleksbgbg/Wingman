namespace Wingman.Container.Strategies
{
    internal class InstanceStrategy : IServiceLocationStrategy
    {
        private readonly object _implementation;

        public InstanceStrategy(object implementation)
        {
            _implementation = implementation;
        }

        public object LocateService(IDependencyRetriever dependencyRetriever)
        {
            return _implementation;
        }
    }
}