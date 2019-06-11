namespace Wingman.ServiceFactory
{
    using Wingman.Container;
    using Wingman.ServiceFactory.Strategies;
    using Wingman.ServiceFactory.Strategies.PerRequest;

    /// <summary> Factory that invokes the internal constructor of <see cref="ServiceFactory"/> and <see cref="ServiceFactoryRegistrar"/>. </summary>
    public static class ServiceFactoryFactory
    {
        public static (IServiceFactoryRegistrar Registrar, IServiceFactory Factory) Create(IDependencyRegistrar dependencyRegistrar, IDependencyRetriever dependencyRetriever)
        {
            RetrievalStrategyStore retrievalStrategyStore = new RetrievalStrategyStore();

            return (new ServiceFactoryRegistrar(dependencyRegistrar,
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