namespace Wingman.Tests.Container
{
    using Wingman.Container;

    using Xunit;

    public class InstanceStrategyTests
    {
        [Fact]
        public void TestReturnsImplementation()
        {
            object expectedService = new object();
            InstanceStrategy strategy = new InstanceStrategy(expectedService);

            object actualService = strategy.LocateService(null);

            Assert.Equal(expectedService, actualService);
        }
    }
}