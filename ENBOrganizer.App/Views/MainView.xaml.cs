using ENBOrganizer.App.ViewModels;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace ENBOrganizer.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        private readonly GameService _gameService;

        public Game SelectedGame { get; set; }

        public MainView()
        {
            InitializeComponent();

            _gameService = ViewModelLocator.GameService;
            _gameService.ItemsChanged += GameService_ItemsChanged;

            LoadGames();
        }

        private void GameService_ItemsChanged(object sender, RepositoryChangedEventArgs e)
        {
            LoadGames();
        }

        private void LoadGames()
        {
            WindowCommands.Items.Clear();

            foreach (Game game in _gameService.GetAll())
            {
                Button button = new Button
                {
                    Content = new Image
                    {
                        Source = System.Drawing.Icon.ExtractAssociatedIcon(game.ExecutablePath).ToImageSource(),
                        Width=24,
                        Height=24,
                    },
                    Tag = game
                };
                button.Click += Button_Click;

                WindowCommands.Items.Add(button);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            _gameService.CurrentGame = (Game)button.Tag;
        }
    }
}