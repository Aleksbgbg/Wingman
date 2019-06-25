namespace Wingman.WpfAppExample
{
    using Wingman.Bootstrapper;
    using Wingman.Container;
    using Wingman.ServiceFactory;
    using Wingman.WpfAppExample.ViewModels;
    using Wingman.WpfAppExample.ViewModels.Interfaces;

    public class AppBootstrapper : BootstrapperBase<IShellViewModel>
    {
        protected override void RegisterViewModels(IDependencyRegistrar dependencyRegistrar)
        {
            dependencyRegistrar.Singleton<IShellViewModel, ShellViewModel>();
        }

        protected override void RegisterFactoryViewModels(IServiceFactoryRegistrar dependencyRegistrar)
        {
            dependencyRegistrar.RegisterPerRequest<IMainViewModel, MainViewModel>();
        }
    }
}