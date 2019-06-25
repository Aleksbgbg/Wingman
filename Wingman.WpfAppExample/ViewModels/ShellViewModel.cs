namespace Wingman.WpfAppExample.ViewModels
{
    using Wingman.ServiceFactory;
    using Wingman.WpfAppExample.ViewModels.Interfaces;

    public sealed class ShellViewModel : ViewModelBase, IShellViewModel
    {
        public ShellViewModel(IServiceFactory serviceFactory)
        {
            DisplayName = "Wingman.WpfAppExample";

            MainViewModel = serviceFactory.Create<IMainViewModel>("Wingman WPF Example");
        }

        public IMainViewModel MainViewModel { get; }
    }
}