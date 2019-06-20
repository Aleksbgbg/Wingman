namespace Wingman.Tests.Container.Strategies
{
    using System;

    using Moq;

    using Wingman.Container.Strategies;

    using Xunit;

    public class PerRequestStrategyTests
    {
        private readonly Mock<IDiStrategy> _diStrategyMock;

        private readonly PerRequestStrategy _perRequestStrategy;

        public PerRequestStrategyTests()
        {
            _diStrategyMock = new Mock<IDiStrategy>();

            _perRequestStrategy = new PerRequestStrategy(_diStrategyMock.Object);
        }

        [Fact]
        public void TestReturnsDiStrategyResult()
        {
            object expectedResult = new object();
            SetupLocateServiceReturns(expectedResult);

            object actualResult = _perRequestStrategy.LocateService();

            Assert.Same(expectedResult, actualResult);
        }

        [Fact]
        public void TestDiLocateCalledOnMethod()
        {
            VerifyDiLocateServiceNotCalled();
            _perRequestStrategy.LocateService();
            VerifyDiLocateServiceCalled();
        }

        private void SetupLocateServiceReturns(object obj)
        {
            _diStrategyMock.Setup(strategy => strategy.LocateService())
                           .Returns(obj);
        }

        private void VerifyDiLocateServiceCalled()
        {
            VerifyDiLocateServiceCalled(Times.Once);
        }

        private void VerifyDiLocateServiceNotCalled()
        {
            VerifyDiLocateServiceCalled(Times.Never);
        }

        private void VerifyDiLocateServiceCalled(Func<Times> times)
        {
            _diStrategyMock.Verify(strategy => strategy.LocateService(), times);
        }
    }
}