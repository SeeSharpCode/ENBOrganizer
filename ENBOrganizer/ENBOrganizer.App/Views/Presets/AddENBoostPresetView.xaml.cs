using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ENBOrganizer.App.Views.Presets
{
    /// <summary>
    /// Interaction logic for AddENBoostPresetView.xaml
    /// </summary>
    public partial class AddENBoostPresetView : UserControl
    {
        public AddENBoostPresetView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(e.Uri.ToString());
            }
            catch (Exception) { }
        }
    }
}
