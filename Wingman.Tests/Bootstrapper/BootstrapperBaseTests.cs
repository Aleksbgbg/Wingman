namespace Wingman.Tests.Bootstrapper
{
    using System;

    using Wingman.Bootstrapper;
    using Wingman.Container;
    using Wingman.ServiceFactory;

    using Xunit;

    public class BootstrapperBaseTests
    {
        private readonly IDependencyRegistrar _dependencyRegistrar;

        private readonly IDependencyRetriever _dependencyRetriever;

        private Bootstrapper _bootstrapper;

        public BootstrapperBaseTests()
        {
            DependencyContainerCreation dependencyContainerCreation = DependencyContainerFactory.Create();

            _dependencyRegistrar = dependencyContainerCreation.Registrar;
            _dependencyRetriever = dependencyContainerCreation.Retriever;
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
            _bootstrapper = new Bootstrapper(_dependencyRegistrar, _dependencyRetriever);
        }

        private void CreateBootstrapperNoRootViewModel()
        {
            _bootstrapper = new Bootstrapper(_dependencyRegistrar, _dependencyRetriever, false);
        }

        private void VerifyRegister<T>()
        {
            Assert.True(_dependencyRegistrar.HasHandler(typeof(T)));
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

            internal Bootstrapper(IDependencyRegistrar dependencyRegistrar,
                                  IDependencyRetriever dependencyRetriever,
                                  bool registerRootViewModel = true)
                    : base(dependencyRegistrar, dependencyRetriever)
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