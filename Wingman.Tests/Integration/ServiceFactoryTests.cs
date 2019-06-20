namespace Wingman.Tests.Integration
{
    using Wingman.Container;
    using Wingman.ServiceFactory;

    using Xunit;

    public class ServiceFactoryTests
    {
        private readonly IDependencyRegistrar _dependencyRegistrar;

        private readonly IServiceFactoryRegistrar _serviceFactoryRegistrar;

        private readonly IServiceFactory _serviceFactory;

        public ServiceFactoryTests()
        {
            DependencyContainerCreation dependencyContainerCreation = DependencyContainerFactory.Create();

            _dependencyRegistrar = dependencyContainerCreation.Registrar;

            ServiceFactoryCreation serviceFactoryCreation = ServiceFactoryFactory.Create(dependencyContainerCreation.Registrar,
                                                                                         dependencyContainerCreation.Retriever);

            _serviceFactoryRegistrar = serviceFactoryCreation.Registrar;
            _serviceFactory = serviceFactoryCreation.Factory;
        }

        [Fact]
        public void TestFromRetriever()
        {
            Service serviceImplementation = RegisterService();

            IService service = RegisterAndMakeFromRetriever<IService>();

            Assert.Same(serviceImplementation, service);
        }

        [Fact]
        public void TestPerRequest()
        {
            Dependency dependency = RegisterDependency();
            object argument = new object();

            ServiceWithDependencies service = RegisterAndMakePerRequest<ServiceWithDependencies>(argument);

            Assert.NotNull(service);
            Assert.Equal(dependency, service.Dependency);
            Assert.Equal(argument, service.Argument);
        }

        private Service RegisterService()
        {
            Service serviceImplementation = new Service();

            _dependencyRegistrar.RegisterInstance(typeof(IService), serviceImplementation);

            return serviceImplementation;
        }

        private Dependency RegisterDependency()
        {
            Dependency dependency = new Dependency();

            _dependencyRegistrar.RegisterInstance(typeof(IDependency), dependency);

            return dependency;
        }

        private TService RegisterAndMakeFromRetriever<TService>()
        {
            _serviceFactoryRegistrar.RegisterFromRetriever<TService>();
            return _serviceFactory.Create<TService>();
        }

        private TService RegisterAndMakePerRequest<TService>(params object[] arguments) where TService : IService
        {
            _serviceFactoryRegistrar.RegisterPerRequest<IService, TService>();
            return (TService)_serviceFactory.Create<IService>(arguments);
        }

        private interface IService { }

        private interface IDependency { }

        private class Dependency : IDependency { }

        private class Service : IService { }

        private class ServiceWithDependencies : IService
        {
            public ServiceWithDependencies(IDependency dependency, object argument)
            {
                Dependency = dependency;
                Argument = argument;
            }

            public IDependency Dependency { get; }

            public object Argument { get; }
        }
    }
}