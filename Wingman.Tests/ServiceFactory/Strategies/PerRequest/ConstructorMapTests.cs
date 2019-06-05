namespace Wingman.Tests.ServiceFactory.Strategies.PerRequest
{
    using System;
    using System.Reflection;

    using Moq;

    using Wingman.ServiceFactory.Strategies.PerRequest;

    using Xunit;

    public class ConstructorMapTests
    {
        private readonly Mock<IConstructorFactory> _constructorFactory;

        private ConstructorMap _constructorMap;

        public ConstructorMapTests()
        {
            _constructorFactory = new Mock<IConstructorFactory>();
        }

        [Fact]
        public void MapsPublicConstructor()
        {
            MapConstructorsOf<ServiceWithPublicConstructor>();

            VerifyMakeConstructorCalled(Times.Once());
        }

        [Fact]
        public void MapsMultiplePublicConstructorsAndNotHiddenOnes()
        {
            MapConstructorsOf<ServiceWithPublicAndHiddenConstructors>();

            VerifyMakeConstructorCalled(Times.Exactly(3));
        }

        [Fact]
        public void DoesNotMapHiddenConstructor()
        {
            Action map = () => MapConstructorsOf<ServiceWithHiddenConstructor>();

            Assert.Throws<InvalidOperationException>(map);
        }

        [Fact]
        public void DoesNotMapStaticConstructor()
        {
            Action map = () => MapConstructorsOf<ServiceWithStaticConstructor>();

            Assert.Throws<InvalidOperationException>(map);
        }

        [Fact]
        public void CallsAcceptsArguments()
        {
            var constructors = SetupConstructors(SetupConstructorAccepts());

            IConstructor constructor = FindBestConstructor();

            Assert.NotNull(constructor);
            VerifyAcceptsArgumentsCalled(constructors[0]);
        }

        [Fact]
        public void FindsCorrectConstructor()
        {
            var constructors = SetupConstructors(SetupConstructorDoesNotAccept(),
                                                 SetupConstructorDoesNotAccept(),
                                                 SetupConstructorAccepts());

            IConstructor constructor = FindBestConstructor();

            Assert.Equal(constructors[2].Object, constructor);
        }

        [Fact]
        public void ThrowsOnAmbiguousConstructors()
        {
            SetupConstructors(SetupConstructorAccepts(),
                              SetupConstructorAccepts());

            Action find = () => FindBestConstructor();

            Assert.Throws<InvalidOperationException>(find);
        }

        private void MapConstructorsOf<T>()
        {
            MapConstructors(typeof(T));
        }

        private void MapConstructors(int quantity)
        {
            Mock<Type> typeMock = new Mock<Type>();
            typeMock.Setup(type => type.GetConstructors(It.IsAny<BindingFlags>()))
                    .Returns(new ConstructorInfo[quantity]);

            MapConstructors(typeMock.Object);
        }

        private void MapConstructors(Type type)
        {
            _constructorMap = new ConstructorMap(_constructorFactory.Object, type);
        }

        private IConstructor FindBestConstructor()
        {
            return _constructorMap.FindBestFitForArguments(null);
        }

        private void VerifyMakeConstructorCalled(Times times)
        {
            _constructorFactory.Verify(factory => factory.MakeConstructor(It.IsAny<ConstructorInfo>()), times);
        }

        private static void VerifyAcceptsArgumentsCalled(Mock<IConstructor> constructorMock)
        {
            constructorMock.Verify(constructor => constructor.AcceptsUserArguments(null));
        }

        private Mock<IConstructor>[] SetupConstructors(params Mock<IConstructor>[] constructors)
        {
            var makeSequence = _constructorFactory.SetupSequence(factory => factory.MakeConstructor(It.IsAny<ConstructorInfo>()));

            foreach (Mock<IConstructor> constructor in constructors)
            {
                makeSequence.Returns(constructor.Object);
            }

            MapConstructors(constructors.Length);

            return constructors;
        }

        private static Mock<IConstructor> SetupConstructorAccepts()
        {
            return SetupConstructor(true);
        }

        private static Mock<IConstructor> SetupConstructorDoesNotAccept()
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

        private class ServiceWithPublicConstructor
        {
            public ServiceWithPublicConstructor(object _) { }
        }

        private class ServiceWithPublicAndHiddenConstructors
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