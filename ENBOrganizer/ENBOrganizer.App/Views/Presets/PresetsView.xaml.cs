using System.Windows;
using System.Windows.Controls;

namespace ENBOrganizer.App.Views.Presets
{
    /// <summary>
    /// Interaction logic for PresetsOverviewView.xaml
    /// </summary>
    public partial class PresetsView : UserControl
    {
        public PresetsView()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddPopup.IsOpen = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPopup.IsOpen = false;
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }
    }
}
