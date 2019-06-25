namespace Wingman.WpfAppExample.ViewModels
{
    using Wingman.WpfAppExample.ViewModels.Interfaces;

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(string appName)
        {
            AppName = appName;
        }

        public string AppName { get; }
    }
}