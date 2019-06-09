namespace Wingman.ServiceFactory
{
    using Wingman.ServiceFactory.Strategies;

    /// <summary> Factory that invokes the internal constructor of <see cref="ServiceFactory"/>. </summary>
    public static class ServiceFactoryFactory
    {
        public static ServiceFactory Create()
        {
            return new ServiceFactory(new RetrievalStrategyStore());
        }
    }
}