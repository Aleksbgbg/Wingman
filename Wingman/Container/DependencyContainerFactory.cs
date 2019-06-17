namespace Wingman.Container
{
    /// <summary> Factory that invokes the internal constructor of <see cref="DependencyContainer"/>. </summary>
    public static class DependencyContainerFactory
    {
        public static DependencyContainerCreation Create()
        {
            DependencyContainer dependencyContainer = new DependencyContainer(new SimpleContainerAdapter());

            return new DependencyContainerCreation(dependencyContainer,
                                                   dependencyContainer,
                                                   dependencyContainer);
        }
    }
}