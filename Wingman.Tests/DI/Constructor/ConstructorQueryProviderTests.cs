namespace Wingman.Tests.DI.Constructor
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Moq;

    using Wingman.DI.Constructor;

    using Xunit;

    public class ConstructorQueryProviderTests
    {
        private readonly Mock<IConstructorFactory> _constructorFactoryMock;

        private readonly ConstructorQueryProvider _constructorQueryProvider;

        public ConstructorQueryProviderTests()
        {
            _constructorFactoryMock = new Mock<IConstructorFactory>();

            _constructorQueryProvider = new ConstructorQueryProvider(_constructorFactoryMock.Object);
        }

        [Fact]
        public void TestMapsPublicConstructor()
        {
            MapConstructorsOf<ServiceWithPublicConstructor>();

            VerifyMakeConstructorCalled<ServiceWithPublicConstructor>(expectedConstructors: 1);
        }

        [Fact]
        public void TestMapsMultiplePublicConstructorsAndNotHiddenOnes()
        {
            MapConstructorsOf<ServiceWithPublicAndHiddenConstructors>();

            VerifyMakeConstructorCalled<ServiceWithPublicAndHiddenConstructors>(expectedConstructors: 3);
        }

        [Fact]
        public void TestDoesNotMapHiddenConstructor()
        {
            MapConstructorsOf<ServiceWithHiddenConstructor>();

            VerifyMakeConstructorCalled<ServiceWithHiddenConstructor>(expectedConstructors: 0);
        }

        [Fact]
        public void TestDoesNotMapStaticConstructor()
        {
            MapConstructorsOf<ServiceWithStaticConstructor>();

            VerifyMakeConstructorCalled<ServiceWithStaticConstructor>(expectedConstructors: 0);
        }

        private void MapConstructorsOf<T>()
        {
            MapConstructors(typeof(T));
        }

        private void MapConstructors(Type type)
        {
            _constructorQueryProvider.QueryPublicInstanceConstructors(type).ToArray();
        }

        private void VerifyMakeConstructorCalled<T>(int expectedConstructors)
        {
            ConstructorInfo[] constructors = typeof(T).GetConstructors();

            Assert.Equal(expectedConstructors, constructors.Length);

            foreach (ConstructorInfo constructor in constructors)
            {
                _constructorFactoryMock.Verify(factory => factory.CreateConstructor(constructor), Times.Once);
            }

            _constructorFactoryMock.VerifyNoOtherCalls();
        }

        private class ServiceWithPublicConstructor
        {
            public ServiceWithPublicConstructor(object _) { }
        }

         class ServiceWithPublicAndHiddenConstructors
        {
            public ServiceWithPublicAndHiddenConstructors() { }

            public ServiceWithPublicAndHiddenConstructors(object _) { }

            public ServiceWithPublicAndHiddenConstructors(object _, object _1) { }

            internal ServiceWithPublicAndHiddenConstructors(object _, object _1, object _2) { }

            private ServiceWithPublicAndHiddenConstructors(object _, object _1, object _2, object _3) { }
        }

        private class ServiceWithHiddenConstructor
        {
            internal ServiceWithHiddenConstructor(object _) { }
        }

        private class ServiceWithStaticConstructor
        {
            private ServiceWithStaticConstructor() { }

            static ServiceWithStaticConstructor() { }
        }
    }
}