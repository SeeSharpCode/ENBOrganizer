using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ENBOrganizer.App.Views
{
    /// <summary>
    /// Interaction logic for PresetFilesView.xaml
    /// </summary>
    public partial class PresetFilesView : UserControl
    {
        public PresetFilesView()
        {
            InitializeComponent();
        }

        // HACK: set the selected TreeViewItem on a right-click
        private void ExtendedTreeView_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem == null)
                return;

            treeViewItem.Focus();
            treeViewItem.IsSelected = true;
            e.Handled = true;
        }

        private static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
    }
}
