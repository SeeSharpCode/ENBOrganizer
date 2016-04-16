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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PresetsOverviewViewModel presetsOverviewViewModel = (PresetsOverviewViewModel)DataContext;

            foreach (Preset preset in presetsOverviewViewModel.Presets)
            {
                Tile tile = new Tile
                {
                    Style = (Style)this.FindResource("LargeTileStyle"),
                    Title = preset.Name,
                    Content = new ToggleSwitch { OffLabel = "Disabled", OnLabel = "Enabled" },
                    Background = new ImageBrush(new BitmapImage(new Uri(@"D:\Pictures\15105-3-1334352572.jpg"))) { Stretch = Stretch.UniformToFill },
                    Tag = preset
                };

                tile.Click += Tile_Click;

                PresetsWrapPanel.Children.Add(tile);
            }

            //Tile tile = new Tile
            //{
            //    Style = (Style)this.FindResource("LargeTileStyle"),
            //    Title = "RealVision ENB",
            //    Content = new ToggleSwitch { OffLabel = "Disabled", OnLabel = "Enabled" },
            //    Background = new ImageBrush(new BitmapImage(new Uri(@"D:\Pictures\15105-3-1334352572.jpg"))) { Stretch = Stretch.UniformToFill }
            //};

            //Tile tile1 = new Tile
            //{
            //    Style = (Style)this.FindResource("LargeTileStyle"),
            //    Title = "Vividan",
            //    Content = new ToggleSwitch { OffLabel = "Disabled", OnLabel = "Enabled" }
            //};

            //Tile tile2 = new Tile
            //{
            //    Style = (Style)this.FindResource("LargeTileStyle"),
            //    Title = "Vividan",
            //    Content = new ToggleSwitch { OffLabel = "Disabled", OnLabel = "Enabled" },
            //    Background = new ImageBrush(new BitmapImage(new Uri(@"D:\Pictures\30936-1-1427218733.jpg"))) { Stretch = Stretch.UniformToFill }
            //};

            //PresetsWrapPanel.Children.Add(tile);
            //PresetsWrapPanel.Children.Add(tile1);
            //PresetsWrapPanel.Children.Add(tile2);
        }

        private void Tile_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            Tile tile = (Tile)sender;
            ViewModelLocator.PresetDetailViewModel.Preset = (Preset)tile.Tag;
            NavigationService.Navigate(new PresetDetailView());
        }
    }
}
