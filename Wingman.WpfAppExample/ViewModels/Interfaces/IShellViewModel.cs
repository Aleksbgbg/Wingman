namespace Wingman.WpfAppExample.ViewModels.Interfaces
{
    public interface IShellViewModel : IViewModelBase
    {
        IMainViewModel MainViewModel { get; }
    }
}