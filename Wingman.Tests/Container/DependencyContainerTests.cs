namespace Wingman.Tests.Container
{
    using Wingman.Container;

    using Xunit;

    public class DependencyContainerTests
    {
        private readonly IDependencyContainer _dependencyContainer;

        public DependencyContainerTests()
        {
            _dependencyContainer = new DependencyContainer();
        }

        [Fact]
        public void TestRegisterHandler()
        {
            _dependencyContainer.RegisterHandler(typeof(string), null, container => string.Empty);
        }
    }
}