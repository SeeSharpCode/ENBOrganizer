using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public interface IPageViewModel 
    {
        ICommand DeleteCommand { get; set; }
        ICommand OpenAddDialogCommand { get; set; }
    }
}