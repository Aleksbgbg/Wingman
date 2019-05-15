namespace Wingman.Tests.ServiceFactory
{
    using System;

    using Moq;

    using Wingman.Container;
    using Wingman.ServiceFactory;

    using Xunit;

    public class ServiceFactoryTests
    {
        private readonly Mock<IDependencyRegistrar> _dependencyRegistrarMock;

        private readonly Mock<IDependencyRetriever> _dependencyRetrieverMock;

        private readonly ServiceFactory _serviceFactory;

        public ServiceFactoryTests()
        {
            _dependencyRegistrarMock = new Mock<IDependencyRegistrar>();

            _dependencyRetrieverMock = new Mock<IDependencyRetriever>();

            _serviceFactory = new ServiceFactory(_dependencyRegistrarMock.Object, _dependencyRetrieverMock.Object);
        }

        [Fact]
        public void TestRegisterThrowsWhenDoesNotHandler()
        {
            SetupHasServiceHandler(false);

            Action register = () => _serviceFactory.Register<IService, Service>();

            Assert.Throws<InvalidOperationException>(register);
        }

        [Fact]
        public void TestRegisterThrowsWhenInterfaceRegisteredTwice()
        {
            SetupHasServiceHandler();

            _serviceFactory.Register<IService, Service>();
            Action registerAgain = () => _serviceFactory.Register<IService, Service>();

            Assert.Throws<InvalidOperationException>(registerAgain);
        }

        [Fact]
        public void TestRegisterThrowsWhenImplementationIsNotConcrete()
        {
            SetupHasServiceHandler();

            Action register = () => _serviceFactory.Register<IService, IService>();

            Assert.Throws<InvalidOperationException>(register);
        }

        [Fact]
        public void TestMakeService()
        {
            IService service = RegisterAndCreate<Service>();

            Assert.NotNull(service);
            Assert.IsType<Service>(service);
        }

        [Fact]
        public void TestMakeThrowsWhenServiceNotRegistered()
        {
            Action makeViewModel = () => _serviceFactory.Make<IService>();

            Assert.Throws<InvalidOperationException>(makeViewModel);
        }

        [Fact]
        public void TestConstructConstructorlessTypeThrows()
        {
            Action construct = () => RegisterAndCreate<ConstructorlessService>();

            Assert.Throws<InvalidOperationException>(construct);
        }

        [Fact]
        public void TestServiceWithCorrectConstructor()
        {
            IService service = RegisterAndCreate<ServiceWithCorrectConstructor>(new object());

            Assert.NotNull(service);
            Assert.IsType<ServiceWithCorrectConstructor>(service);
        }

        [Fact]
        public void TestServiceWithMultipleConstructors()
        {
            const int parameterCount = 2;

            var service = RegisterAndCreateStrongType<ServiceWithMultiplePublicConstructors>(FillObjects(parameterCount));

            Assert.NotNull(service);
            Assert.Equal(parameterCount, service.ParameterCount);
        }

        [Fact]
        public void TestServiceWithMultipleConstructorsThrowsWhenTooManyConstructorsMatch()
        {
            const int parameterCount = 1;

            Action create = () => RegisterAndCreateStrongType<ServiceWithMultiplePublicConstructors>(FillObjects(parameterCount));

            Assert.Throws<InvalidOperationException>(create);
        }

        [Fact]
        public void TestServiceWithNoPublicConstructorThrows()
        {
            Action create = () => RegisterAndCreate<ServiceWithInternalConstructor>();

            Assert.Throws<InvalidOperationException>(create);
        }

        private void SetupHasServiceHandler(bool value = true)
        {
            _dependencyRegistrarMock.Setup(registrar => registrar.HasHandler(typeof(IService), null))
                                    .Returns(value);
        }

        private TImplementation RegisterAndCreateStrongType<TImplementation>(params object[] parameters) where TImplementation : IService
        {
            return (TImplementation)RegisterAndCreate<TImplementation>(parameters);
        }

        private IService RegisterAndCreate<TImplementation>(params object[] parameters) where TImplementation : IService
        {
            SetupHasServiceHandler();

            _serviceFactory.Register<IService, TImplementation>();

            return _serviceFactory.Make<IService>(parameters);
        }

        private static object[] FillObjects(int quantity)
        {
            object[] objects = new object[quantity];

            for (int index = 0; index < objects.Length; index++)
            {
                objects[index] = new object();
            }

            return objects;
        }

        private interface IService
        {
        }

        private class Service : IService
        {
        }

        private class ServiceWithCorrectConstructor : IService
        {
            public ServiceWithCorrectConstructor(object parameter)
            {
            }
        }

        private class ServiceWithMultiplePublicConstructors : IService
        {
            public ServiceWithMultiplePublicConstructors(object parameter)
            {
                ParameterCount = 1;
            }

            public ServiceWithMultiplePublicConstructors(object parameter, object parameter2)
            {
                ParameterCount = 2;
            }

            public int ParameterCount { get; }
        }

        private class ServiceWithInternalConstructor : IService
        {
            internal ServiceWithInternalConstructor(object parameter)
            {
            }
        }

        private class ConstructorlessService : IService
        {
            private ConstructorlessService()
            {
            }
        }
    }
}