namespace Wingman.Container.Strategies
{
    internal class SingletonStrategy : IServiceLocationStrategy
    {
        private IDiStrategy _diStrategy;

        private object _service;

        internal SingletonStrategy(IDiStrategy diStrategy)
        {
            _diStrategy = diStrategy;
        }

        public object LocateService()
        {
            if (_service == null)
            {
                _service = _diStrategy.LocateService();
                _diStrategy = null;
            }

            return _service;
        }
    }
}