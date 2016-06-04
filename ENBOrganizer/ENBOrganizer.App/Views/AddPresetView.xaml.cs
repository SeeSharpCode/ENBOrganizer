using System.Windows.Controls;

namespace ENBOrganizer.App.Views
{
    /// <summary>
    /// Interaction logic for ImportPresetView.xaml
    /// </summary>
    public partial class AddPresetView : UserControl
    {
        public AddPresetView()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddPopup.IsOpen = true;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddPopup.IsOpen = false;
        }
    }
}
