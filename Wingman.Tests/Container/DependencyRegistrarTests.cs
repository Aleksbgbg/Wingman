namespace Wingman.Tests.Container
{
    using System;
    using System.Linq.Expressions;

    using Moq;

    using Wingman.Container;

    using Xunit;

    public class DependencyRegistrarTests
    {
        private const string ServiceKey = "Key";

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

            _dependencyRegistrar.RegisterInstance(typeof(IService), service, ServiceKey);

            VerifyRegisterStrategy();
        }

        [Fact]
        public void TestRegisterSingleton()
        {
            SetupCreateSingleton();

            _dependencyRegistrar.RegisterSingleton(typeof(IService), typeof(Service), ServiceKey);

            VerifyRegisterStrategy();
        }

        [Fact]
        public void TestRegisterPerRequest()
        {
            SetupCreatePerRequest();

            _dependencyRegistrar.RegisterPerRequest(typeof(IService), typeof(Service), ServiceKey);

            VerifyRegisterStrategy();
        }

        [Fact]
        public void TestRegisterHandler()
        {
            Func<IDependencyRetriever, object> handler = _ => new object();
            SetupRegisterHandler(handler);

            _dependencyRegistrar.RegisterHandler(typeof(IService), handler, ServiceKey);

            VerifyRegisterStrategy();
        }

        [Fact]
        public void TestUnregisterHandler()
        {
            _dependencyRegistrar.UnregisterHandler(typeof(IService), ServiceKey);

            VerifyUnregisterHandler();
        }

        [Fact]
        public void TestHasHandlerTrue()
        {
            SetupHasHandler(true);

            bool hasHandler = _dependencyRegistrar.HasHandler(typeof(IService), ServiceKey);

            Assert.True(hasHandler);
        }

        [Fact]
        public void TestHasHandlerFalse()
        {
            SetupHasHandler(false);

            bool hasHandler = _dependencyRegistrar.HasHandler(typeof(IService), ServiceKey);

            Assert.False(hasHandler);
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
            _serviceEntryStoreMock.Setup(store => store.HasHandler(MatchesDefaultServiceEntry()))
                                  .Returns(expectedResult);
        }

        private void VerifyRegisterStrategy()
        {
            _serviceEntryStoreMock.Verify(store => store.InsertHandler(MatchesDefaultServiceEntry(), _locationStrategy));
        }

        private void VerifyUnregisterHandler()
        {
            _serviceEntryStoreMock.Verify(store => store.RemoveHandler(MatchesDefaultServiceEntry()));
        }

        private static ServiceEntry MatchesDefaultServiceEntry()
        {
            return It.Is<ServiceEntry>(entry => entry.GetHashCode() == DefaultServiceEntry.GetHashCode());
        }

        private static ServiceEntry DefaultServiceEntry { get; } = new ServiceEntry(typeof(IService), ServiceKey);

        private interface IService { }

        private class Service : IService { }
    }
}