using System.Windows.Controls;

namespace ENBOrganizer.App.Views
{
    /// <summary>
    /// Interaction logic for AddBinaryView.xaml
    /// </summary>
    public partial class AddBinaryView : UserControl
    {
        public AddBinaryView()
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
