namespace Wingman.Tests.DI.Constructor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using Wingman.DI.Constructor;

    using Xunit;

    public class ConstructorMapTests
    {
        private readonly Mock<IConstructorQueryProvider> _constructorQueryProviderMock;

        private ConstructorMap _constructorMap;

        public ConstructorMapTests()
        {
            _constructorQueryProviderMock = new Mock<IConstructorQueryProvider>();
        }

        [Fact]
        public void TestCallsQueryPublicInstanceConstructors()
        {
            SetupConstructorCount(1);

            CreateConstructorMap();

            VerifyQueryPublicInstanceConstructors();
        }

        [Fact]
        public void TestThrowsOnZeroConstructors()
        {
            SetupConstructorCount(0);

            Action map = () => CreateConstructorMap();

            Assert.Throws<InvalidOperationException>(map);
        }

        [Fact]
        public void TestFindBestConstructorCallsAcceptsArguments()
        {
            var constructors = SetupConstructors(SetupConstructorEligible());

            IConstructor constructor = FindBestConstructorForArguments();

            Assert.NotNull(constructor);
            VerifyAcceptsArgumentsCalled(constructors[0]);
        }

        [Fact]
        public void TestFindsElibigbleConstructor()
        {
            var constructors = SetupConstructors(SetupConstructorNotEligible(),
                                                 SetupConstructorNotEligible(),
                                                 SetupConstructorEligible());

            IConstructor constructor = FindBestConstructorForArguments();

            Assert.Equal(constructors[2].Object, constructor);
        }

        [Fact]
        public void TestThrowsOnMultipleEligibleConstructors()
        {
            SetupConstructors(SetupConstructorEligible(),
                              SetupConstructorEligible());

            Action find = () => FindBestConstructorForArguments();

            Assert.Throws<InvalidOperationException>(find);
        }

        [Fact]
        public void TestReturnsSmallestArgumentCountConstructor()
        {
            const int expectedParameterCount = 12;
            SetupConstructors(SetupConstructorWithParameterCount(50),
                              SetupConstructorWithParameterCount(expectedParameterCount),
                              SetupConstructorWithParameterCount(18));

            IConstructor constructor = FindBestConstructorForDi();

            Assert.Equal(expectedParameterCount, constructor.ParameterCount);
        }

        private void SetupConstructorCount(int count)
        {
            Mock<IEnumerable<IConstructor>> enumerableMock = new Mock<IEnumerable<IConstructor>>();
            enumerableMock.Setup(enumerable => enumerable.GetEnumerator())
                          .Returns(((IEnumerable<IConstructor>)new IConstructor[count]).GetEnumerator());

            _constructorQueryProviderMock.Setup(provider => provider.QueryPublicInstanceConstructors(typeof(SomeType)))
                                         .Returns(enumerableMock.Object);
        }

        private IConstructor FindBestConstructorForArguments()
        {
            CreateConstructorMap();

            return _constructorMap.FindBestConstructorForArguments(null);
        }

        private IConstructor FindBestConstructorForDi()
        {
            CreateConstructorMap();

            return _constructorMap.FindBestConstructorForDi();
        }

        private void CreateConstructorMap()
        {
            _constructorMap = new ConstructorMap(_constructorQueryProviderMock.Object, typeof(SomeType));
        }

        private static Mock<IConstructor> SetupConstructorEligible()
        {
            return SetupConstructor(true);
        }

        private static Mock<IConstructor> SetupConstructorNotEligible()
        {
            return SetupConstructor(false);
        }

        private static Mock<IConstructor> SetupConstructor(bool accepts)
        {
            Mock<IConstructor> constructorMock = new Mock<IConstructor>();
            constructorMock.Setup(constructor => constructor.AcceptsUserArguments(null))
                           .Returns(accepts);

            return constructorMock;
        }

        private static Mock<IConstructor> SetupConstructorWithParameterCount(int count)
        {
            Mock<IConstructor> constructorMock = new Mock<IConstructor>();
            constructorMock.Setup(constructor => constructor.ParameterCount)
                           .Returns(count);

            return constructorMock;
        }

        private Mock<IConstructor>[] SetupConstructors(params Mock<IConstructor>[] constructors)
        {
            _constructorQueryProviderMock.Setup(provider => provider.QueryPublicInstanceConstructors(typeof(SomeType))).Returns(constructors.Select(constructor => constructor.Object).ToArray());

            return constructors;
        }

        private void VerifyQueryPublicInstanceConstructors()
        {
            _constructorQueryProviderMock.Verify(provider => provider.QueryPublicInstanceConstructors(typeof(SomeType)), Times.Once);
        }

        private static void VerifyAcceptsArgumentsCalled(Mock<IConstructor> constructorMock)
        {
            constructorMock.Verify(constructor => constructor.AcceptsUserArguments(null));
        }

        private class SomeType { }
    }
}