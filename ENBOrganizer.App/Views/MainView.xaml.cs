using ENBOrganizer.App.Converters;
using ENBOrganizer.App.ViewModels;
using ENBOrganizer.Model.Entities;
using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ENBOrganizer.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        private GamesViewModel _gamesViewModel;

        public MainView()
        {
            InitializeComponent();
        }

        //private void _gamesViewModel_PropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        //{
        //    if (propertyChangedEventArgs.PropertyName == "Games")
        //        LoadGameButtons();
        //}

        //private void LoadGameButtons()
        //{
        //    GamesViewModel gamesViewModel = ((MainViewModel)DataContext).GamesViewModel;

        //    foreach (Game game in gamesViewModel.Games)
        //    {
        //        PathToIconConverter pathToIconConverter = new PathToIconConverter();

        //        ToggleButton toggleButton = new ToggleButton
        //        {
        //            Content = new Image
        //            {
        //                Source = (ImageSource)(pathToIconConverter.Convert(game.ExecutablePath, Type.GetType("ImageSource"), null, null)),
        //                Width = 24,
        //                Height = 24
        //            },

        //            IsChecked = game.Equals(gamesViewModel.CurrentGame)
        //        };

        //        WindowCommands.Items.Add(toggleButton);
        //    }
               
        //}
    }
}