namespace Wingman.Tests.Container
{
    using System.Linq;

    using Moq;

    using Wingman.Container;

    using Xunit;

    public class DependencyRetrieverExtensionsTests
    {
        private readonly Mock<IDependencyRetriever> _dependencyRetrieverMock;

        private readonly IDependencyRetriever _dependencyRetriever;

        public DependencyRetrieverExtensionsTests()
        {
            _dependencyRetrieverMock = new Mock<IDependencyRetriever>();

            _dependencyRetriever = _dependencyRetrieverMock.Object;
        }

        [Fact]
        public void TestGetInstance()
        {
            SetupServiceInstance();

            IService service = _dependencyRetriever.GetInstance<IService>(Service.ServiceKey);

            Assert.NotNull(service);
            Assert.IsType<Service>(service);
        }

        [Fact]
        public void TestGetAllInstances()
        {
            SetupServiceInstances();

            IService[] services = _dependencyRetriever.GetAllInstances<IService>()
                                                      .ToArray();

            Assert.IsType<Service>(services[0]);
            Assert.IsType<ServiceType2>(services[1]);
        }

        private void SetupServiceInstance()
        {
            _dependencyRetrieverMock.Setup(retriever => retriever.GetInstance(typeof(IService), Service.ServiceKey)).Returns(new Service());
        }

        private void SetupServiceInstances()
        {
            _dependencyRetrieverMock.Setup(retriever => retriever.GetAllInstances(typeof(IService)))
                                    .Returns(new IService[]
                                    {
                                        new Service(),
                                        new ServiceType2()
                                    });
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
        }
    }
}