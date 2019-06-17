namespace Wingman.ServiceFactory
{
    public class ServiceFactoryPair
    {
        public ServiceFactoryPair(IServiceFactoryRegistrar registrar, IServiceFactory factory)
        {
            Registrar = registrar;
            Factory = factory;
        }

        public IServiceFactoryRegistrar Registrar { get; }

        public IServiceFactory Factory { get; }
    }
}