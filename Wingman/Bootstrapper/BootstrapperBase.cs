namespace Wingman.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Caliburn.Micro;

    using Wingman.Container;
    using Wingman.ServiceFactory;
    using Wingman.Utilities;

    /// <inheritdoc/>
    /// <typeparam name="TRootViewModel"> The ViewModel to display for the root view. </typeparam>
    public abstract class BootstrapperBase<TRootViewModel> : BootstrapperBase
    {
        private readonly IDependencyRegistrar _dependencyRegistrar;

        private readonly IDependencyRetriever _dependencyRetriever;

        protected BootstrapperBase()
        {
            DependencyContainerCreation dependencyContainerCreation = DependencyContainerFactory.Create();

            _dependencyRegistrar = dependencyContainerCreation.Registrar;
            _dependencyRetriever = dependencyContainerCreation.Retriever;

            Initialize();
        }

#if !DEBUG
        protected BootstrapperBase(IDependencyRegistrar dependencyRegistrar, IDependencyRetriever dependencyRetriever)
        {
            _dependencyRegistrar = dependencyRegistrar;
            _dependencyRetriever = dependencyRetriever;

            Initialize();
        }
#endif

#if DEBUG
        protected BootstrapperBase(IDependencyRegistrar dependencyRegistrar, IDependencyRetriever dependencyRetriever)
        {
            _dependencyRegistrar = dependencyRegistrar;
            _dependencyRetriever = dependencyRetriever;
        }
#endif

        protected sealed override void BuildUp(object instance)
        {
            _dependencyRetriever.BuildUp(instance);
        }

        protected sealed override IEnumerable<object> GetAllInstances(Type service)
        {
            return _dependencyRetriever.GetAllInstances(service);
        }

        protected sealed override object GetInstance(Type service, string key)
        {
            return _dependencyRetriever.GetInstance(service, key);
        }

        protected sealed override void Configure()
        {
            ServiceFactoryCreation serviceFactoryCreation = ServiceFactoryFactory.Create(_dependencyRegistrar, _dependencyRetriever);

            RegisterCommonDependencies(serviceFactoryCreation.Factory);

            RegisterViewModels(_dependencyRegistrar);
            CheckRootViewModelRegistered();

            RegisterServices(_dependencyRegistrar);
            RegisterFactoryViewModels(serviceFactoryCreation.Registrar);
            RegisterFactoryServices(serviceFactoryCreation.Registrar);
        }

        /// <summary> Override to register ViewModels in the provided dependency registrar. Required as ViewModels are always needed. </summary>
        protected abstract void RegisterViewModels(IDependencyRegistrar dependencyRegistrar);

        /// <summary> Override to register any required services in the provided dependency registrar. </summary>
        protected virtual void RegisterServices(IDependencyRegistrar dependencyRegistrar)
        {
        }

        /// <summary> Override to register ViewModels in the provided dependency registrar, for runtime retrieval. </summary>
        protected virtual void RegisterFactoryViewModels(IServiceFactoryRegistrar dependencyRegistrar)
        {
        }

        /// <summary> Override to register any required services in the provided dependency registrar, for runtime retrieval. </summary>
        protected virtual void RegisterFactoryServices(IServiceFactoryRegistrar dependencyRegistrar)
        {
        }

        protected sealed override void OnStartup(object sender, StartupEventArgs e)
        {
            OnStartupBeforeDisplayRootView(sender, e);

            DisplayRootViewFor<TRootViewModel>();

            OnStartupAfterDisplayRootView(sender, e);
        }

        /// <summary> Override to perform any actions before the root ViewModel is displayed. </summary>
        protected virtual void OnStartupBeforeDisplayRootView(object sender, StartupEventArgs e)
        {
        }

        /// <summary> Override to perform any actions after the root ViewModel is displayed. </summary>
        protected virtual void OnStartupAfterDisplayRootView(object sender, StartupEventArgs e)
        {
        }

        private void RegisterCommonDependencies(IServiceFactory serviceFactory)
        {
            _dependencyRegistrar.Singleton<IWindowManager, WindowManager>();
            _dependencyRegistrar.Instance(serviceFactory);
        }

        private void CheckRootViewModelRegistered()
        {
            if (!_dependencyRegistrar.HasHandler<TRootViewModel>())
            {
                ThrowHelper.Throw.BootstrapperBase.RootViewModelNotRegistered(typeof(TRootViewModel), nameof(RegisterViewModels));
            }
        }
    }
}