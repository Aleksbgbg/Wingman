namespace Wingman.Tests.Bootstrapper
{
    using System;

    using Wingman.Bootstrapper;
    using Wingman.Container;
    using Wingman.ServiceFactory;

    using Xunit;

    public class BootstrapperBaseTests
    {
        private readonly DependencyContainer _dependencyContainer;

        private Bootstrapper _bootstrapper;

        public BootstrapperBaseTests()
        {
            _dependencyContainer = DependencyContainerFactory.Create();
        }

        [Fact]
        public void TestThrowsIfRootViewModelNotRegistered()
        {
            Action create = () => CreateBootstrapperNoRootViewModel();

            Assert.Throws<InvalidOperationException>(create);
        }

        [Fact]
        public void RegistersWindowManager()
        {
            CreateBootstrapper();
            VerifyRegister<Caliburn.Micro.IWindowManager>();
        }

        [Fact]
        public void RegistersServiceFactory()
        {
            CreateBootstrapper();
            VerifyRegister<IServiceFactory>();
        }

        private void CreateBootstrapper()
        {
            _bootstrapper = new Bootstrapper(_dependencyContainer);
        }

        private void CreateBootstrapperNoRootViewModel()
        {
            _bootstrapper = new Bootstrapper(_dependencyContainer, false);
        }

        private void VerifyRegister<T>()
        {
            Assert.True(_dependencyContainer.HasHandler(typeof(T)));
        }

        private interface IRootViewModel
        {
        }

        private class RootViewModel : IRootViewModel
        {
        }

        private class Bootstrapper : BootstrapperBase<IRootViewModel>
        {
            private readonly bool _registerRootViewModel;

            internal Bootstrapper(DependencyContainerBase dependencyContainer, bool registerRootViewModel = true) : base(dependencyContainer, dependencyContainer)
            {
                _registerRootViewModel = registerRootViewModel;

                Configure();
            }

            protected override void RegisterViewModels(IDependencyRegistrar dependencyRegistrar)
            {
                if (_registerRootViewModel)
                {
                    dependencyRegistrar.Singleton<IRootViewModel, RootViewModel>();
                }
            }
        }
    }
}