namespace Wingman.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Caliburn.Micro;

    using Wingman.Container;
    using Wingman.ServiceFactory;

    /// <inheritdoc/>
    public abstract class BootstrapperBase<TRootViewModel> : BootstrapperBase<DependencyContainerBase, TRootViewModel>
    {
        protected BootstrapperBase() : base(DependencyContainerFactory.Create())
        {
        }
    }

    /// <inheritdoc/>
    /// <typeparam name="TContainer"> An implementer of <see cref="DependencyContainerBase"/> to use as a dependency registrar and container. </typeparam>
    /// <typeparam name="TRootViewModel"> The ViewModel to display for the root view. </typeparam>
    public abstract class BootstrapperBase<TContainer, TRootViewModel> : BootstrapperBase
            where TContainer : IDependencyRegistrar, IDependencyRetriever
    {
        private readonly TContainer _dependencyContainer;

        private readonly ServiceFactory _serviceFactory;

#if DEBUG
        private protected BootstrapperBase(TContainer dependencyContainer, object _)
        {
            _dependencyContainer = dependencyContainer;

            Configure();
        }
#endif

        protected BootstrapperBase(TContainer dependencyContainer)
        {
            _dependencyContainer = dependencyContainer;
            _serviceFactory = new ServiceFactory(dependencyContainer, dependencyContainer);

            Initialize();
        }

        protected sealed override void BuildUp(object instance)
        {
            _dependencyContainer.BuildUp(instance);
        }

        protected sealed override IEnumerable<object> GetAllInstances(Type service)
        {
            return _dependencyContainer.GetAllInstances(service);
        }

        protected sealed override object GetInstance(Type service, string key)
        {
            return _dependencyContainer.GetInstance(service, key);
        }

        protected sealed override void Configure()
        {
            RegisterCommonDependencies();

            RegisterViewModels(_dependencyContainer);
            RegisterServices(_dependencyContainer);

            RegisterFactoryViewModels(_serviceFactory);
            RegisterFactoryServices(_serviceFactory);
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

        private void RegisterCommonDependencies()
        {
            _dependencyContainer.Instance<IServiceFactory>(_serviceFactory);
            _dependencyContainer.Singleton<IWindowManager, WindowManager>();
        }
    }
}