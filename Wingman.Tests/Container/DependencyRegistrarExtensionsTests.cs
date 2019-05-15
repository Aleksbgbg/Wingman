namespace Wingman.Tests.Container
{
    using System;
    using System.Reflection;

    using Moq;

    using Wingman.Container;

    using Xunit;

    public class DependencyRegistrarExtensionsTests
    {
        private readonly Mock<IDependencyRegistrar> _dependencyRegistrarMock;

        private readonly IDependencyRegistrar _dependencyRegistrar;

        public DependencyRegistrarExtensionsTests()
        {
            _dependencyRegistrarMock = new Mock<IDependencyRegistrar>();

            _dependencyRegistrar = _dependencyRegistrarMock.Object;
        }

        [Fact]
        public void TestRegisterInstance()
        {
            Service service = new Service();

            _dependencyRegistrar.Instance(service, service.InstanceKey);

            VerifyRegisterInstanceCalled(service);
        }

        [Fact]
        public void TestRegisterSingleton()
        {
            _dependencyRegistrar.Singleton<IService, Service>(Service.ServiceKey);

            VerifyRegisterSingletonCalled();
        }

        [Fact]
        public void TestRegisterSingletonConcreteType()
        {
            _dependencyRegistrar.Singleton<Service>(Service.ServiceKey);

            VerifyRegisterSingletonCalledForConcreteType();
        }

        [Fact]
        public void TestRegisterPerRequest()
        {
            _dependencyRegistrar.PerRequest<IService, Service>(Service.ServiceKey);

            VerifyRegisterPerRequestCalled();
        }

        [Fact]
        public void TestRegisterPerRequestConcreteType()
        {
            _dependencyRegistrar.PerRequest<Service>(Service.ServiceKey);

            VerifyRegisterPerRequestCalledConcreteType();
        }

        [Fact]
        public void TestRegisterHandler()
        {
            Func<IDependencyRetriever, object> handler = registrar => new Service();

            _dependencyRegistrar.Handler<IService>(handler, Service.ServiceKey);

            VerifyRegisterHandlerCalled(handler);
        }

        [Fact]
        public void TestRegisterAllTypesOf()
        {
            _dependencyRegistrar.RegisterAllTypesOf<IService>(Assembly.GetAssembly(typeof(IService)), key: Service.ServiceKey);

            VerifyRegisterSingletonCalled();
            VerifyRegisterSingletonCalled<IService, ServiceType2>();
            _dependencyRegistrarMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void TestRegisterAllTypesOfWithFilter()
        {
            _dependencyRegistrar.RegisterAllTypesOf<IService>(Assembly.GetAssembly(typeof(IService)), type => type != typeof(ServiceType2), Service.ServiceKey);

            VerifyRegisterSingletonCalled();
            _dependencyRegistrarMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void TestUnregister()
        {
            _dependencyRegistrar.Unregister<IService>(Service.ServiceKey);

            VerifyUnregisterHandlerCalled();
        }

        [Fact]
        public void TestHasHandler()
        {
            SetupHasHandler();

            bool hasHandler = _dependencyRegistrar.HasHandler<IService>(Service.ServiceKey);

            Assert.True(hasHandler);
            VerifyHasHandlerCalled();
        }

        private void SetupHasHandler()
        {
            _dependencyRegistrarMock.Setup(registrar => registrar.HasHandler(typeof(IService), Service.ServiceKey)).Returns(true);
        }

        private void VerifyRegisterInstanceCalled(Service service)
        {
            _dependencyRegistrarMock.Verify(registrar => registrar.RegisterInstance(typeof(Service), service.InstanceKey, service));
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
            _dependencyRegistrarMock.Verify(registrar => registrar.RegisterSingleton(typeof(TService), Service.ServiceKey, typeof(TImplementation)));
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
            _dependencyRegistrarMock.Verify(registrar => registrar.RegisterPerRequest(typeof(TService), Service.ServiceKey, typeof(Service)));
        }

        private void VerifyRegisterHandlerCalled(Func<IDependencyRetriever, object> handler)
        {
            _dependencyRegistrarMock.Verify(registrar => registrar.RegisterHandler(typeof(IService), Service.ServiceKey, handler));
        }

        private void VerifyUnregisterHandlerCalled()
        {
            _dependencyRegistrarMock.Verify(registrar => registrar.UnregisterHandler(typeof(IService), Service.ServiceKey));
        }

        private void VerifyHasHandlerCalled()
        {
            _dependencyRegistrarMock.Verify(registrar => registrar.HasHandler(typeof(IService), Service.ServiceKey));
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