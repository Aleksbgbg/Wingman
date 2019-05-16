namespace Wingman.Tests.Bootstrapper
{
    using Caliburn.Micro;

    using Wingman.Bootstrapper;
    using Wingman.Container;
    using Wingman.ServiceFactory;

    using Xunit;

    public class BootstrapperBaseTests
    {
        private readonly DependencyContainer _dependencyContainer;

        private readonly Bootstrapper _bootstrapper;

        public BootstrapperBaseTests()
        {
            _dependencyContainer = DependencyContainerFactory.Create();

            _bootstrapper = new Bootstrapper(_dependencyContainer);
        }

        [Fact]
        public void RegistersWindowManager()
        {
            VerifyRegister<IWindowManager>();
        }

        [Fact]
        public void RegistersServiceFactory()
        {
            VerifyRegister<IServiceFactory>();
        }

        private void VerifyRegister<T>()
        {
            Assert.True(_dependencyContainer.HasHandler(typeof(T)));
        }

        private class Bootstrapper : BootstrapperBase<DependencyContainerBase, object>
        {
            internal Bootstrapper(DependencyContainerBase dependencyContainer) : base(dependencyContainer, null)
            {
            }

            protected override void RegisterViewModels(IDependencyRegistrar dependencyRegistrar)
            {
            }
        }
    }
}