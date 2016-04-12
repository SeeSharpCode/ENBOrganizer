using ENBOrganizer.App.ViewModels;
using ENBOrganizer.Model.Entities;
using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace ENBOrganizer.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();

            MainViewModel mainViewModel = (MainViewModel)DataContext;

            foreach (Game game in mainViewModel.GamesViewModel.Games)
            {
                WindowCommands.Items.Add(new Button { Content = game.Name });
            }
        }
    }
}