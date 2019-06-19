namespace Wingman.Tests.Container
{
    using System;
    using System.Reflection;

    using Moq;

    using Wingman.Container;

    using Xunit;

    public class DependencyRegistrarExtensionsTests
    {
        private const string DefaultServiceKey = "Key";

        private readonly Mock<IDependencyRegistrar> _dependencyRegistrarMock;

        public DependencyRegistrarExtensionsTests()
        {
            _dependencyRegistrarMock = new Mock<IDependencyRegistrar>();
        }

        [Fact]
        public void TestRegisterAllValidServiceTypes()
        {
            RegisterAllTypes();

            VerifyRegistered<DefaultService>();
            VerifyRegistered<OtherService>();
            VerifyNotRegistered<AbstractService>();
            VerifyNotRegistered<IgnoredService>();
        }

        [Fact]
        public void TestRegisterDefaultServiceOnly()
        {
            RegisterAllTypes(type => type == typeof(DefaultService));

            VerifyRegistered<DefaultService>();
            VerifyNotRegistered<OtherService>();
            VerifyNotRegistered<AbstractService>();
            VerifyNotRegistered<IgnoredService>();
        }

        private void RegisterAllTypes(Func<Type, bool> matchFilter = null)
        {
            DependencyRegistrarExtensions.RegisterAllTypesOf<IService>(_dependencyRegistrarMock.Object, Assembly.GetExecutingAssembly(), matchFilter, DefaultServiceKey);
        }

        private void VerifyRegistered<T>() where T : IService
        {
            VerifyRegistered<T>(Times.Once);
        }

        private void VerifyNotRegistered<T>()
        {
            VerifyRegistered<T>(Times.Never);
        }

        private void VerifyRegistered<T>(Func<Times> times)
        {
            _dependencyRegistrarMock.Verify(registrar => registrar.RegisterSingleton(typeof(IService), typeof(T), DefaultServiceKey), times);
        }

        private interface IService { }

        private class DefaultService : IService { }

        private class OtherService : IService { }

        private abstract class AbstractService : IService { }

        private class IgnoredService { }
    }
}