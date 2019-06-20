namespace Wingman.Tests.Container.Strategies
{
    using Wingman.Container.Strategies;

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