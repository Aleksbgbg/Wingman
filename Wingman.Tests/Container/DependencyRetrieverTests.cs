namespace Wingman.Tests.Container
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using Wingman.Container;
    using Wingman.Container.Entries;
    using Wingman.Container.Strategies;

    using Xunit;

    public class DependencyRetrieverTests
    {
        private const string DefaultServiceKey = "Key";

        private static readonly Type DefaultServiceType = typeof(IService);

        private static readonly ServiceEntry DefaultServiceEntry = new ServiceEntry(DefaultServiceType, DefaultServiceKey);

        private static readonly ServiceEntry NullKeyServiceEntry = new ServiceEntry(DefaultServiceType, null);

        private readonly Mock<IServiceEntryStore> _serviceEntryStoreMock;

        private readonly DependencyRetriever _dependencyRetriever;

        public DependencyRetrieverTests()
        {
            _serviceEntryStoreMock = new Mock<IServiceEntryStore>();

            _dependencyRetriever = new DependencyRetriever(_serviceEntryStoreMock.Object);
        }

        [Fact]
        public void TestReturnsEntryWhenHasHandler()
        {
            IService expectedObject = SetupIServiceHandler();

            IService actualObject = GetIServiceInstance();

            Assert.Same(expectedObject, actualObject);
        }

        [Fact]
        public void TestThrowsWhenMultipleHandlersAvailable()
        {
            SetupIServiceHandlers(5);

            Action getInstance = () => GetIServiceInstance();

            Assert.Throws<InvalidOperationException>(getInstance);
        }

        [Fact]
        public void TestThrowsWhenServiceNullAndNoHandlers()
        {
            Action getInstance = () => GetInstance(null);

            Assert.Throws<InvalidOperationException>(getInstance);
        }

        [Fact]
        public void TestThrowsWhenNoHandlersButServiceNotNull()
        {
            Action getInstance = () => GetIServiceInstance();

            Assert.Throws<InvalidOperationException>(getInstance);
        }

        [Fact]
        public void TestInstantiatesFactoryFunc()
        {
            Func<IService> expectedFactory = SetupFactoryFunc();

            Func<IService> actualFactory = GetFactoryFuncInstance();

            Assert.Same(expectedFactory(), actualFactory());
        }

        [Fact]
        public void TestGetAllInstances()
        {
            IEnumerable<object> expectedCollection = SetupIServiceHandlers(5);

            IEnumerable<object> actualCollection = GetAllInstances();

            Assert.Equal(expectedCollection, actualCollection);
        }

        [Fact]
        public void TestInstantiatesCollection()
        {
            IEnumerable<IService> expectedCollection = SetupInjectServiceCollection();

            IEnumerable<IService> actualCollection = GetCollectionInstance();

            Assert.Equal(expectedCollection, actualCollection);
        }

        [Fact]
        public void TestGetAllInstancesThrowsWhenNoHandlers()
        {
            Action getAllInstances = () => GetAllInstances();

            Assert.Throws<InvalidOperationException>(getAllInstances);
        }

        [Fact]
        public void TestBuildUp()
        {
            BuildUpType buildUpType = new BuildUpType();
            IService expectedService = SetupIServiceHandler(key: null);

            BuildUp(buildUpType);

            buildUpType.VerifyInjectedCorrectly(expectedService);
        }

        [Fact]
        public void TestBuildUpSelectsFirstInstanceWhenMultipleAvailable()
        {
            BuildUpType buildUpType = new BuildUpType();
            IEnumerable<object> services = SetupIServiceHandlers(5);

            BuildUp(buildUpType);

            Assert.Same(services.First(), buildUpType.Service);
        }

        private Func<IService> SetupFactoryFunc()
        {
            SetupIServiceHandler();

            return () => _dependencyRetriever.GetInstance<IService>(DefaultServiceKey);
        }

        private IEnumerable<IService> SetupInjectServiceCollection()
        {
            return new List<IService>
            {
                SetupIServiceHandler(key: null)
            };
        }

        private IService SetupIServiceHandler(string key = DefaultServiceKey)
        {
            SetupHasIServiceHandler();

            Service service = new Service();

            _serviceEntryStoreMock.Setup(store => store.RetrieveHandlers(new ServiceEntry(DefaultServiceType, key)))
                                  .Returns(new IServiceLocationStrategy[]
                                  {
                                      SetupServiceLocationStrategy(service)
                                  });

            return service;
        }

        private IEnumerable<object> SetupIServiceHandlers(int count)
        {
            SetupHasIServiceHandler();

            IServiceLocationStrategy[] strategies = new IServiceLocationStrategy[count];
            for (int index = 0; index < strategies.Length; ++index)
            {
                strategies[index] = SetupServiceLocationStrategy(new Service());
            }

            _serviceEntryStoreMock.Setup(store => store.RetrieveHandlers(NullKeyServiceEntry))
                                  .Returns(strategies);

            return strategies.Select(strategy => strategy.LocateService());
        }

        private IServiceLocationStrategy SetupServiceLocationStrategy(Service service)
        {
            Mock<IServiceLocationStrategy> serviceLocationStrategyMock = new Mock<IServiceLocationStrategy>();
            serviceLocationStrategyMock.Setup(strategy => strategy.LocateService())
                                       .Returns(service);

            return serviceLocationStrategyMock.Object;
        }

        private void SetupHasIServiceHandler()
        {
            _serviceEntryStoreMock.Setup(store => store.HasHandler(DefaultServiceEntry))
                                  .Returns(true);

            _serviceEntryStoreMock.Setup(store => store.HasHandler(NullKeyServiceEntry))
                                  .Returns(true);
        }

        private IService GetIServiceInstance()
        {
            return (IService)GetInstance(DefaultServiceType);
        }

        private Func<IService> GetFactoryFuncInstance()
        {
            return (Func<IService>)GetInstance(typeof(Func<IService>));
        }

        private IEnumerable<IService> GetCollectionInstance()
        {
            return ((object[])GetInstance(typeof(IEnumerable<IService>))).Cast<IService>();
        }

        private object GetInstance(Type service)
        {
            return _dependencyRetriever.GetInstance(service, DefaultServiceKey);
        }

        private IEnumerable<object> GetAllInstances()
        {
            return _dependencyRetriever.GetAllInstances(DefaultServiceType);
        }

        private void BuildUp(object instance)
        {
            _dependencyRetriever.BuildUp(instance);
        }

        private interface IService { }

        private class Service : IService { }

        private interface ISomeInterface { }

        private class BuildUpType
        {
            public IService Service { get; set; }

            public IService ServicePrivateGetter { private get; set; }

            public IService ServiceNoSetter { get; }

            public ISomeInterface ServiceNotRegistered { get; set; }

            internal IService ServiceInternal { get; set; }

            public void VerifyInjectedCorrectly(IService expectedService)
            {
                Assert.Same(expectedService, Service);
                Assert.Same(expectedService, ServicePrivateGetter);
                Assert.Null(ServiceNoSetter);
                Assert.Null(ServiceNotRegistered);
                Assert.Null(ServiceInternal);
            }
        }
    }
}