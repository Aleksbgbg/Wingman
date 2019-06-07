namespace Wingman.ServiceFactory
{
    using Wingman.Container;
    using Wingman.ServiceFactory.Strategies;
    using Wingman.ServiceFactory.Strategies.PerRequest;

    /// <summary> Factory that invokes the internal constructor of <see cref="ServiceFactory"/>. </summary>
    public static class ServiceFactoryFactory
    {
        public static ServiceFactory Create(IDependencyRegistrar dependencyRegistrar, IDependencyRetriever dependencyRetriever)
        {
            return new ServiceFactory(dependencyRegistrar,
                                      new RetrievalStrategyStore(new RetrievalStrategyFactory(dependencyRetriever,
                                                                                              new ArgumentBuilderFactory(dependencyRetriever),
                                                                                              new ConstructorMapFactory(new ConstructorFactory())
                                                                                              )
                                                                 )
                                      );
        }
    }
}