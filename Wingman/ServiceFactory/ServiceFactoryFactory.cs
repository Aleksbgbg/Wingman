namespace Wingman.ServiceFactory
{
    using Wingman.Container;

    /// <summary> Factory that invokes the internal constructor of <see cref="ServiceFactory"/>. </summary>
    public static class ServiceFactoryFactory
    {
        public static ServiceFactory Create(IDependencyRegistrar dependencyRegistrar)
        {
            return new ServiceFactory(dependencyRegistrar, null);
        }
    }
}