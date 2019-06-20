namespace Wingman.ServiceFactory
{
    public class ServiceFactoryCreation
    {
        public ServiceFactoryCreation(IServiceFactoryRegistrar registrar, IServiceFactory factory)
        {
            Registrar = registrar;
            Factory = factory;
        }

        public IServiceFactoryRegistrar Registrar { get; }

        public IServiceFactory Factory { get; }
    }
}