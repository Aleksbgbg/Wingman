namespace Wingman.Container
{
    using Wingman.Container.Entries;
    using Wingman.Container.Strategies;
    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;

    /// <summary> Factory that invokes the internal constructor of <see cref="DependencyContainer"/>. </summary>
    public static class DependencyContainerFactory
    {
        public static DependencyContainerCreation Create()
        {
            ServiceEntryStore serviceEntryStore = new ServiceEntryStore();

            DependencyRetriever dependencyRetriever = new DependencyRetriever(serviceEntryStore);

            DependencyRegistrar dependencyRegistrar = new DependencyRegistrar(serviceEntryStore,
                                                                              new LocationStrategyFactory(dependencyRetriever,
                                                                                                          new ConstructorMapFactory(new ConstructorQueryProvider(new ConstructorFactory())),
                                                                                                          new ArgumentBuilderFactory(dependencyRetriever),
                                                                                                          new ObjectBuilderFactory()
                                                                              )
            );

            return new DependencyContainerCreation(dependencyRegistrar,
                                                   dependencyRetriever,
                                                   dependencyRetriever);
        }
    }
}