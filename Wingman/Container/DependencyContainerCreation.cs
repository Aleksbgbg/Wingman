namespace Wingman.Container
{
    public class DependencyContainerCreation
    {
        public DependencyContainerCreation(IDependencyRegistrar registrar, IDependencyRetriever retriever, IDependencyActivator activator)
        {
            Registrar = registrar;
            Retriever = retriever;
            Activator = activator;
        }

        public IDependencyRegistrar Registrar { get; }

        public IDependencyRetriever Retriever { get; }

        public IDependencyActivator Activator { get; }
    }
}