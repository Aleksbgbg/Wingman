namespace Wingman.ServiceFactory
{
    using Wingman.Container;
    using Wingman.DI;
    using Wingman.DI.ArgumentBuilder;
    using Wingman.DI.Constructor;
    using Wingman.ServiceFactory.Strategies;

    /// <summary> Factory that invokes the internal constructor of <see cref="ServiceFactory"/> and <see cref="ServiceFactoryRegistrar"/>. </summary>
    public static class ServiceFactoryFactory
    {
        public static ServiceFactoryCreation Create(IDependencyRegistrar dependencyRegistrar, IDependencyRetriever dependencyRetriever)
        {
            RetrievalStrategyStore retrievalStrategyStore = new RetrievalStrategyStore();

            return new ServiceFactoryCreation(new ServiceFactoryRegistrar(dependencyRegistrar,
                                                                          new RetrievalStrategyFactory(dependencyRetriever,
                                                                                                       new UserArgumentBuilderFactory(dependencyRetriever),
                                                                                                       new ConstructorMapFactory(new ConstructorQueryProvider(new ConstructorFactory()))
                                                                          ),
                                                                          retrievalStrategyStore
                                              ),
                                              new ServiceFactory(retrievalStrategyStore)
            );
        }
    }
}