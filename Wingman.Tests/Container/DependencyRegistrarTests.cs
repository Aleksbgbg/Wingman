namespace Wingman.Tests.Container
{
    using System;
    using System.Linq.Expressions;

    using Moq;

    using Wingman.Container;
    using Wingman.Container.Entries;
    using Wingman.Container.Strategies;

    using Xunit;

    public class DependencyRegistrarTests
    {
        private const string DefaultServiceKey = "Key";

        private static readonly ServiceEntry DefaultServiceEntry = new ServiceEntry(typeof(IService), DefaultServiceKey);

        private readonly Mock<IServiceEntryStore> _serviceEntryStoreMock;

        private readonly Mock<ILocationStrategyFactory> _locationStrategyFactoryMock;

        private readonly DependencyRegistrar _dependencyRegistrar;

        private readonly IServiceLocationStrategy _locationStrategy;

        public DependencyRegistrarTests()
        {
            _serviceEntryStoreMock = new Mock<IServiceEntryStore>();

            _locationStrategyFactoryMock = new Mock<ILocationStrategyFactory>();

            _dependencyRegistrar = new DependencyRegistrar(_serviceEntryStoreMock.Object, _locationStrategyFactoryMock.Object);

            _locationStrategy = new Mock<IServiceLocationStrategy>().Object;
        }

        [Fact]
        public void TestRegisterInstance()
        {
            IService service = new Service();
            SetupCreateInstance(service);

            RegisterInstance(service);

            VerifyRegisterStrategy();
        }

        [Fact]
        public void TestRegisterSingleton()
        {
            SetupCreateSingleton();

            RegisterSingleton();

            VerifyRegisterStrategy();
        }

        [Fact]
        public void TestRegisterPerRequest()
        {
            SetupCreatePerRequest();

            RegisterPerRequest();

            VerifyRegisterStrategy();
        }

        [Fact]
        public void TestRegisterHandler()
        {
            Func<IDependencyRetriever, object> handler = _ => new object();
            SetupRegisterHandler(handler);

            RegisterHandler(handler);

            VerifyRegisterStrategy();
        }

        [Fact]
        public void TestUnregisterHandler()
        {
            UnregisterHandler();

            VerifyUnregisterHandler();
        }

        [Fact]
        public void TestHasHandlerTrue()
        {
            SetupHasHandler(true);

            Assert.True(HasHandler());
        }

        [Fact]
        public void TestHasHandlerFalse()
        {
            SetupHasHandler(false);

            Assert.False(HasHandler());
        }

        [Fact]
        public void TestDuplicateRegistrationDoesNotThrow()
        {
            SetupHasHandler(true);

            RegisterInstance(null);
            RegisterSingleton();
            RegisterPerRequest();
            RegisterHandler();
        }

        private void SetupCreateInstance(IService service)
        {
            SetupReturnsLocationStrategy(factory => factory.CreateInstance(service));
        }

        private void SetupCreateSingleton()
        {
            SetupReturnsLocationStrategy(factory => factory.CreateSingleton(typeof(Service)));
        }

        private void SetupCreatePerRequest()
        {
            SetupReturnsLocationStrategy(factory => factory.CreatePerRequest(typeof(Service)));
        }

        private void SetupRegisterHandler(Func<IDependencyRetriever, object> handler)
        {
            SetupReturnsLocationStrategy(factory => factory.CreateHandler(handler));
        }

        private void SetupReturnsLocationStrategy(Expression<Func<ILocationStrategyFactory, IServiceLocationStrategy>> expression)
        {
            _locationStrategyFactoryMock.Setup(expression)
                                        .Returns(_locationStrategy);
        }

        private void SetupHasHandler(bool expectedResult)
        {
            _serviceEntryStoreMock.Setup(store => store.HasHandler(DefaultServiceEntry))
                                  .Returns(expectedResult);
        }

        private void RegisterInstance(IService implementation)
        {
            _dependencyRegistrar.RegisterInstance(typeof(IService), implementation, DefaultServiceKey);
        }

        private void RegisterSingleton()
        {
            _dependencyRegistrar.RegisterSingleton(typeof(IService), typeof(Service), DefaultServiceKey);
        }

        private void RegisterPerRequest()
        {
            _dependencyRegistrar.RegisterPerRequest(typeof(IService), typeof(Service), DefaultServiceKey);
        }

        private void RegisterHandler()
        {
            RegisterHandler(_ => new object());
        }

        private void RegisterHandler(Func<IDependencyRetriever, object> handler)
        {
            _dependencyRegistrar.RegisterHandler(typeof(IService), handler, DefaultServiceKey);
        }

        private bool HasHandler()
        {
            return _dependencyRegistrar.HasHandler(typeof(IService), DefaultServiceKey);
        }

        private void UnregisterHandler()
        {
            _dependencyRegistrar.UnregisterHandler(typeof(IService), DefaultServiceKey);
        }

        private void VerifyRegisterStrategy()
        {
            _serviceEntryStoreMock.Verify(store => store.InsertHandler(DefaultServiceEntry, _locationStrategy));
        }

        private void VerifyUnregisterHandler()
        {
            _serviceEntryStoreMock.Verify(store => store.RemoveHandler(DefaultServiceEntry));
        }

        private interface IService { }

        private class Service : IService { }
    }
}