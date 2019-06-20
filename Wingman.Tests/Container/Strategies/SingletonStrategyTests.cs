namespace Wingman.Tests.Container.Strategies
{
    using System.Collections.Generic;

    using Moq;

    using Wingman.Container.Strategies;

    using Xunit;

    public class SingletonStrategyTests
    {
        private readonly Mock<IDiStrategy> _diStrategyMock;

        private readonly SingletonStrategy _singletonStrategy;

        public SingletonStrategyTests()
        {
            _diStrategyMock = new Mock<IDiStrategy>();
            _diStrategyMock.Setup(strategy => strategy.LocateService())
                           .Returns(new object());

            _singletonStrategy = new SingletonStrategy(_diStrategyMock.Object);
        }

        [Theory]
        [InlineData(10)]
        public void TestReturnsSameObject(int callTimes)
        {
            object expectedValue = new object();
            SetupLocateServiceReturns(expectedValue);

            IEnumerable<object> invocationResults = CallLocateServiceMultipleTimes(callTimes);

            Assert.All(invocationResults, actualValue => Assert.Same(expectedValue, actualValue));
        }

        [Theory]
        [InlineData(10)]
        public void TestCallsLocateServiceFirstTimeButNotSubsequently(int subsequentCalls)
        {
            _singletonStrategy.LocateService();
            CallLocateServiceMultipleTimes(subsequentCalls);

            VerifyLocateDiServiceCalledOnce();
        }

        private void SetupLocateServiceReturns(object obj)
        {
            _diStrategyMock.Setup(strategy => strategy.LocateService())
                           .Returns(obj);
        }

        private List<object> CallLocateServiceMultipleTimes(int callTimes)
        {
            List<object> callResults = new List<object>();

            for (int call = 0; call < callTimes; ++call)
            {
                callResults.Add(_singletonStrategy.LocateService());
            }

            return callResults;
        }

        private void VerifyLocateDiServiceCalledOnce()
        {
            _diStrategyMock.Verify(strategy => strategy.LocateService(), Times.Once);
        }
    }
}