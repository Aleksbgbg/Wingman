namespace Wingman.ServiceFactory
{
    using Wingman.Container;
    using Wingman.DI;
    using Wingman.ServiceFactory.Strategies;

    /// <summary> Factory that invokes the internal constructor of <see cref="ServiceFactory"/> and <see cref="ServiceFactoryRegistrar"/>. </summary>
    public static class ServiceFactoryFactory
    {
        public static ServiceFactoryCreation Create(IDependencyRegistrar dependencyRegistrar, IDependencyRetriever dependencyRetriever)
        {
            RetrievalStrategyStore retrievalStrategyStore = new RetrievalStrategyStore();

            return new ServiceFactoryCreation(new ServiceFactoryRegistrar(dependencyRegistrar,
                                                                          new RetrievalStrategyFactory(dependencyRetriever,
                                                                                                       new ArgumentBuilderFactory(dependencyRetriever),
                                                                                                       new ConstructorMapFactory(new ConstructorFactory())
                                                                          ),
                                                                          retrievalStrategyStore
                                              ),
                                              new ServiceFactory(retrievalStrategyStore)
            );
        }
    }
}