namespace Wingman.Tests.DI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using Wingman.DI;

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

            MapConstructors();

            VerifyQueryPublicInstanceConstructors();
        }

        [Fact]
        public void TestThrowsOnZeroConstructors()
        {
            SetupConstructorCount(0);

            Action map = () => MapConstructors();

            Assert.Throws<InvalidOperationException>(map);
        }

        [Fact]
        public void TestFindBestConstructorCallsAcceptsArguments()
        {
            var constructors = SetupConstructors(SetupConstructorEligible());

            IConstructor constructor = FindBestConstructor();

            Assert.NotNull(constructor);
            VerifyAcceptsArgumentsCalled(constructors[0]);
        }

        [Fact]
        public void TestFindsElibigbleConstructor()
        {
            var constructors = SetupConstructors(SetupConstructorNotEligible(),
                                                 SetupConstructorNotEligible(),
                                                 SetupConstructorEligible());

            IConstructor constructor = FindBestConstructor();

            Assert.Equal(constructors[2].Object, constructor);
        }

        [Fact]
        public void TestThrowsOnMultipleEligibleConstructors()
        {
            SetupConstructors(SetupConstructorEligible(),
                              SetupConstructorEligible());

            Action find = () => FindBestConstructor();

            Assert.Throws<InvalidOperationException>(find);
        }

        private void SetupConstructorCount(int count)
        {
            Mock<IEnumerable<IConstructor>> enumerableMock = new Mock<IEnumerable<IConstructor>>();
            enumerableMock.Setup(enumerable => enumerable.GetEnumerator())
                          .Returns(((IEnumerable<IConstructor>)new IConstructor[count]).GetEnumerator());

            _constructorQueryProviderMock.Setup(provider => provider.QueryPublicInstanceConstructors(typeof(SomeType)))
                                         .Returns(enumerableMock.Object);
        }

        private void MapConstructors()
        {
            _constructorMap = new ConstructorMap(_constructorQueryProviderMock.Object, typeof(SomeType));
        }

        private void VerifyQueryPublicInstanceConstructors()
        {
            _constructorQueryProviderMock.Verify(provider => provider.QueryPublicInstanceConstructors(typeof(SomeType)), Times.Once);
        }

        private Mock<IConstructor>[] SetupConstructors(params Mock<IConstructor>[] constructors)
        {
            _constructorQueryProviderMock.Setup(provider => provider.QueryPublicInstanceConstructors(typeof(SomeType)))
                                         .Returns(constructors.Select(constructor => constructor.Object).ToArray());

            return constructors;
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

        private IConstructor FindBestConstructor()
        {
            MapConstructors();

            return _constructorMap.FindBestFitForArguments(null);
        }

        private static void VerifyAcceptsArgumentsCalled(Mock<IConstructor> constructorMock)
        {
            constructorMock.Verify(constructor => constructor.AcceptsUserArguments(null));
        }

        private class SomeType { }
    }
}