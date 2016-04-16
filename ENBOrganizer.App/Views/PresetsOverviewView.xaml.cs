using ENBOrganizer.App.ViewModels;
using ENBOrganizer.Model.Entities;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ENBOrganizer.App.Views
{
    /// <summary>
    /// Interaction logic for PresetsOverviewView.xaml
    /// </summary>
    public partial class PresetsOverviewView : Page
    {
        public PresetsOverviewView()
        {
            InitializeComponent();
        }

        private void Tile_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            Tile tile = (Tile)sender;
            
            ViewModelLocator locator = (ViewModelLocator)FindResource("ViewModelLocator");
            locator.PresetDetailViewModel.Preset = (Preset)tile.Tag;

            NavigationService.Navigate(new PresetDetailView());
        }
    }
}
