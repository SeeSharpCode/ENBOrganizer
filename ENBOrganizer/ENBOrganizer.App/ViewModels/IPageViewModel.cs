using ENBOrganizer.Domain.Entities;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public interface IPageViewModel 
    {
        Game CurrentGame { get; }
        ICommand DeleteCommand { get; set; }
        bool IsAddDialogOpen { get; set; }
        string Name { get; }
        ICommand OpenAddDialogCommand { get; set; }
    }
}