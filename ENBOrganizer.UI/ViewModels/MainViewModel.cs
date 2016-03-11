using System.Windows.Input;
using ENBOrganizer.UI.Views;
using ENBOrganizer.Util;

namespace ENBOrganizer.UI.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ICommand ManageGamesCommand { get; set; }

        public MainViewModel()
        {
            ManageGamesCommand = new ActionCommand(ShowManageGamesView, () => true);
        }

        private void ShowManageGamesView()
        {
            new ManageGamesView().ShowDialog();
        }
    }
}
