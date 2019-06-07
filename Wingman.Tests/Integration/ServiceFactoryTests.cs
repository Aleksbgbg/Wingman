namespace Wingman.Tests.Integration
{
    using Wingman.Container;
    using Wingman.ServiceFactory;

    using Xunit;

    public class ServiceFactoryTests
    {
        private readonly DependencyContainer _dependencyContainer;

        private readonly ServiceFactory _serviceFactory;

        public ServiceFactoryTests()
        {
            _dependencyContainer = DependencyContainerFactory.Create();
            _serviceFactory = ServiceFactoryFactory.Create(_dependencyContainer, _dependencyContainer);
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

            _dependencyContainer.RegisterInstance(typeof(IService), serviceImplementation);

            return serviceImplementation;
        }

        private Dependency RegisterDependency()
        {
            Dependency dependency = new Dependency();

            _dependencyContainer.RegisterInstance(typeof(IDependency), dependency);

            return dependency;
        }

        private TService RegisterAndMakeFromRetriever<TService>()
        {
            _serviceFactory.RegisterFromRetriever<TService>();
            return _serviceFactory.Make<TService>();
        }

        private TService RegisterAndMakePerRequest<TService>(params object[] arguments) where TService : IService
        {
            _serviceFactory.RegisterPerRequest<IService, TService>();
            return (TService)_serviceFactory.Make<IService>(arguments);
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