using ENBOrganizer.App.Messages;
using ENBOrganizer.App.ViewModels;
using ENBOrganizer.App.Views;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ENBOrganizer.App
{
    public class DialogService
    {
        private readonly ViewModelLocator _viewModelLocator;

        public DialogService()
        {
            _viewModelLocator = (ViewModelLocator)App.Current.Resources["ViewModelLocator"];
        }

        public void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowWarningDialog(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public string ShowOpenFileDialog(string title, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };

            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : string.Empty;
        }

        public List<string> ShowOpenFilesDialog(string title, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                Multiselect = true
            };

            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileNames.ToList() : new List<string>();
        }

        public string ShowFolderBrowserDialog(string title)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog { Description = title };

            return folderBrowserDialog.ShowDialog() == DialogResult.OK ? folderBrowserDialog.SelectedPath : string.Empty;
        }

        public void ShowDialog(DialogName dialogName)
        {
            Messenger.Default.Send(new DialogMessage(dialogName, DialogAction.Open));
        }

        public void CloseDialog(DialogName dialogName)
        {
            Messenger.Default.Send(new DialogMessage(dialogName, DialogAction.Close));
        }

        public async Task<object> ShowInputDialog(string prompt, string hostName, string defaultValue = null)
        {
            _viewModelLocator.InputViewModel.Prompt = prompt;
            _viewModelLocator.InputViewModel.Name = defaultValue;

            InputView inputDialog = new InputView
            {
                DataContext = _viewModelLocator.InputViewModel
            };

            object input = await DialogHost.Show(inputDialog, hostName);

            return input;
        }
    }
}
