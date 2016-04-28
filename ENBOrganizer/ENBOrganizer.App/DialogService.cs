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
    public class DialogService
    {
        private readonly MetroWindow _mainWindow = System.Windows.Application.Current.MainWindow as MetroWindow;

        public async Task<string> ShowInputDialog(string title, string message)
        {
            return await _mainWindow.ShowInputAsync(title, message);
        }

        public async Task<MessageDialogResult> ShowErrorDialog(string message)
        {
            return await _mainWindow.ShowMessageAsync("Error", message);
        }

        public string PromptForFile(string title, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = title, 
                Filter = filter
            };

            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : string.Empty;
        }

        public List<string> PromptForFiles(string title, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                Multiselect = true
            };

            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileNames.ToList() : new List<string>();
        }

        public string PromptForFolder(string title)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog { Description = title };

            return folderBrowserDialog.ShowDialog() == DialogResult.OK ? folderBrowserDialog.SelectedPath : string.Empty;
        }

        public void ShowAddGameDialog()
        {
            Messenger.Default.Send(new DialogMessage(DialogActions.Open));
        }

        public static void CloseAddGameDialog()
        {
            Messenger.Default.Send(new DialogMessage(DialogActions.Close));
        }
    }
}
