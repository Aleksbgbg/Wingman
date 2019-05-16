namespace Wingman.Container
{
    /// <summary> Factory that invokes the internal constructor of <see cref="DependencyContainer"/>. </summary>
    public static class DependencyContainerFactory
    {
        public static DependencyContainer Create()
        {
            return new DependencyContainer(new SimpleContainerAdapter());
        }
    }
}