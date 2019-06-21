namespace Wingman.Tests.DI
{
    using System;

    using Moq;

    using Wingman.DI;

    using Xunit;

    public class ConstructorCandidateEvaluatorTests
    {
        private readonly Mock<IConstructorQueryProvider> _constructorQueryProviderMock;

        private readonly ConstructorCandidateEvaluator _constructorCandidateEvaluator;

        public ConstructorCandidateEvaluatorTests()
        {
            _constructorQueryProviderMock = new Mock<IConstructorQueryProvider>();

            _constructorCandidateEvaluator = new ConstructorCandidateEvaluator(_constructorQueryProviderMock.Object);
        }

        [Fact]
        public void TestReturnsSmallestArgumentCountConstructor()
        {
            const int expectedParameterCount = 12;
            SetupConstructors(SetupConstructorWithParameterCount(50),
                              SetupConstructorWithParameterCount(expectedParameterCount),
                              SetupConstructorWithParameterCount(18));

            IConstructor constructor = FindBestConstructor();

            Assert.Equal(expectedParameterCount, constructor.ParameterCount);
        }

        [Fact]
        public void TestThrowsWhenNoConstructors()
        {
            SetupConstructors();

            Action find = () => FindBestConstructor();

            Assert.Throws<InvalidOperationException>(find);
        }

        private void SetupConstructors(params IConstructor[] constructors)
        {
            _constructorQueryProviderMock.Setup(provider => provider.QueryPublicInstanceConstructors(typeof(SomeType)))
                                         .Returns(constructors);
        }

        private IConstructor SetupConstructorWithParameterCount(int count)
        {
            Mock<IConstructor> constructorMock = new Mock<IConstructor>();
            constructorMock.Setup(constructor => constructor.ParameterCount)
                           .Returns(count);

            return constructorMock.Object;
        }

        private IConstructor FindBestConstructor()
        {
            return _constructorCandidateEvaluator.FindBestConstructorForDi(typeof(SomeType));
        }

        private class SomeType { }
    }
}