namespace Wingman.Tests.Container
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Caliburn.Micro;

    using Moq;

    using Wingman.Container;

    using Xunit;

    public class DependencyContainerTests
    {
        private readonly Mock<ISimpleContainer> _simpleContainerMock;

        private readonly DependencyContainer _dependencyContainer;

        public DependencyContainerTests()
        {
            _simpleContainerMock = new Mock<ISimpleContainer>();

            _dependencyContainer = new DependencyContainer(_simpleContainerMock.Object);
        }

        [Fact]
        public void TestRegisterInstance()
        {
            Service service = new Service();

            _dependencyContainer.Instance(service, service.InstanceKey);

            VerifyRegisterInstanceCalled(service);
        }

        [Fact]
        public void TestRegisterSingleton()
        {
            _dependencyContainer.Singleton<IService, Service>(Service.ServiceKey);

            VerifyRegisterSingletonCalled();
        }

        [Fact]
        public void TestRegisterSingletonConcreteType()
        {
            _dependencyContainer.Singleton<Service>(Service.ServiceKey);

            VerifyRegisterSingletonCalledForConcreteType();
        }

        [Fact]
        public void TestRegisterPerRequest()
        {
            _dependencyContainer.PerRequest<IService, Service>(Service.ServiceKey);

            VerifyRegisterPerRequestCalled();
        }

        [Fact]
        public void TestRegisterPerRequestConcreteType()
        {
            _dependencyContainer.PerRequest<Service>(Service.ServiceKey);

            VerifyRegisterPerRequestCalledConcreteType();
        }

        [Fact]
        public void TestRegisterHandler()
        {
            Func<IDependencyRetriever, object> handler = registrar => new Service();

            _dependencyContainer.Handler<IService>(handler, Service.ServiceKey);

            VerifyRegisterHandlerCalled(handler);
        }

        [Fact]
        public void TestRegisterAllTypesOf()
        {
            _dependencyContainer.RegisterAllTypesOf<IService>(Assembly.GetAssembly(typeof(IService)), key: Service.ServiceKey);

            VerifyRegisterSingletonCalled();
            VerifyRegisterSingletonCalled<IService, ServiceType2>();
            _simpleContainerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void TestRegisterAllTypesOfWithFilter()
        {
            _dependencyContainer.RegisterAllTypesOf<IService>(Assembly.GetAssembly(typeof(IService)), type => type != typeof(ServiceType2), Service.ServiceKey);

            VerifyRegisterSingletonCalled();
            _simpleContainerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void TestUnregister()
        {
            _dependencyContainer.Unregister<IService>(Service.ServiceKey);

            VerifyUnregisterHandlerCalled();
        }

        [Fact]
        public void TestHasHandler()
        {
            SetupHasHandler();

            bool hasHandler = _dependencyContainer.HasHandler<IService>(Service.ServiceKey);

            Assert.True(hasHandler);
            VerifyHasHandlerCalled();
        }

        [Fact]
        public void TestGetInstance()
        {
            SetupServiceInstance();

            IService service = _dependencyContainer.GetInstance<IService>(Service.ServiceKey);

            Assert.NotNull(service);
            Assert.IsType<Service>(service);
        }

        [Fact]
        public void TestGetAllInstances()
        {
            SetupServiceInstances();

            IService[] services = _dependencyContainer.GetAllInstances<IService>().ToArray();

            Assert.IsType<Service>(services[0]);
            Assert.IsType<ServiceType2>(services[1]);
        }

        private void SetupServiceInstance()
        {
            _simpleContainerMock.Setup(retriever => retriever.GetInstance(typeof(IService), Service.ServiceKey)).Returns(new Service());
        }

        private void SetupServiceInstances()
        {
            _simpleContainerMock.Setup(retriever => retriever.GetAllInstances(typeof(IService)))
                                .Returns(new IService[]
                                {
                                    new Service(),
                                    new ServiceType2()
                                });
        }

        private void SetupHasHandler()
        {
            _simpleContainerMock.Setup(registrar => registrar.HasHandler(typeof(IService), Service.ServiceKey)).Returns(true);
        }

        private void VerifyRegisterInstanceCalled(Service service)
        {
            _simpleContainerMock.Verify(registrar => registrar.RegisterInstance(typeof(Service), service.InstanceKey, service));
        }

        private void VerifyRegisterSingletonCalled()
        {
            VerifyRegisterSingletonCalled<IService>();
        }

        private void VerifyRegisterSingletonCalledForConcreteType()
        {
            VerifyRegisterSingletonCalled<Service>();
        }

        private void VerifyRegisterSingletonCalled<TService>()
        {
            VerifyRegisterSingletonCalled<TService, Service>();
        }

        private void VerifyRegisterSingletonCalled<TService, TImplementation>()
        {
            _simpleContainerMock.Verify(registrar => registrar.RegisterSingleton(typeof(TService), Service.ServiceKey, typeof(TImplementation)));
        }

        private void VerifyRegisterPerRequestCalled()
        {
            VerifyRegisterPerRequestCalled<IService>();
        }

        private void VerifyRegisterPerRequestCalledConcreteType()
        {
            VerifyRegisterPerRequestCalled<Service>();
        }

        private void VerifyRegisterPerRequestCalled<TService>()
        {
            _simpleContainerMock.Verify(registrar => registrar.RegisterPerRequest(typeof(TService), Service.ServiceKey, typeof(Service)));
        }

        private void VerifyRegisterHandlerCalled(Func<IDependencyRetriever, object> handler)
        {
            _simpleContainerMock.Verify(registrar => registrar.RegisterHandler(typeof(IService), Service.ServiceKey, It.IsAny<Func<SimpleContainer, object>>()));
        }

        private void VerifyUnregisterHandlerCalled()
        {
            _simpleContainerMock.Verify(registrar => registrar.UnregisterHandler(typeof(IService), Service.ServiceKey));
        }

        private void VerifyHasHandlerCalled()
        {
            _simpleContainerMock.Verify(registrar => registrar.HasHandler(typeof(IService), Service.ServiceKey));
        }

        private interface IService
        {
        }

        private class ServiceType2 : IService
        {
        }

        private class Service : IService
        {
            public static string ServiceKey { get; } = "SomeKey";

            public string InstanceKey { get; set; } = ServiceKey;
        }
    }
}