using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;

namespace ENBOrganizer.App
{
    public static class DialogService
    {
        public static async Task<string> ShowInputDialog(string title, string message)
        {
            MetroWindow window = Application.Current.MainWindow as MetroWindow;
            return await window.ShowInputAsync(title, message);
        }
    }
}
