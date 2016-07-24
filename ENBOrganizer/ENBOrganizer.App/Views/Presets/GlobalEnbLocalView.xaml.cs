using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ENBOrganizer.App.Views.Presets
{
    /// <summary>
    /// Interaction logic for GlobalPresetView.xaml
    /// </summary>
    public partial class GlobalEnbLocalView : UserControl
    {
        public GlobalEnbLocalView()
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
