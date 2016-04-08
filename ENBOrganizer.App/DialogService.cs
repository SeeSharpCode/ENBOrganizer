using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ENBOrganizer.App
{
    public static class DialogService
    {
        public static async Task<string> ShowInputDialog(string title, string message)
        {
            MetroWindow window = System.Windows.Application.Current.MainWindow as MetroWindow;
            return await window.ShowInputAsync(title, message);
        }

        public static string ShowOpenFileDialog(string title, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = title, 
                Filter = filter
            };

            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : string.Empty;
        }
    }
}
