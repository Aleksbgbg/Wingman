namespace Wingman.Container.Strategies
{
    internal class PerRequestStrategy : IServiceLocationStrategy
    {
        private readonly IDiStrategy _diStrategy;

        internal PerRequestStrategy(IDiStrategy diStrategy)
        {
            _diStrategy = diStrategy;
        }

        public object LocateService()
        {
            return _diStrategy.LocateService();
        }
    }
}