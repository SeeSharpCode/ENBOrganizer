using ENBOrganizer.App.Messages;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public static string PromptForFile(string title, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = title, 
                Filter = filter
            };

            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : string.Empty;
        }

        public static List<string> PromptForFiles(string title, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                Multiselect = true
            };

            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileNames.ToList() : new List<string>();
        }

        public static string PromptForFolder(string title)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog { Description = title };

            return folderBrowserDialog.ShowDialog() == DialogResult.OK ? folderBrowserDialog.SelectedPath : string.Empty;
        }

        public static void ShowAddGameDialog()
        {
            Messenger.Default.Send(new DialogMessage(DialogActions.Open));
        }

        public static void CloseAddGameDialog()
        {
            Messenger.Default.Send(new DialogMessage(DialogActions.Close));
        }
    }
}
