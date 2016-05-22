using ENBOrganizer.App.Messages;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ENBOrganizer.App
{
    public class DialogService
    {
        public void ShowYesOrNoDialog(string title, string message)
        {
            throw new NotImplementedException();
        }

        public void ShowErrorDialog(string message)
        {
            throw new NotImplementedException();
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

        public void ShowDialog(DialogName dialogName)
        {
            Messenger.Default.Send(new DialogMessage(dialogName, DialogAction.Open));
        }

        public void CloseDialog(DialogName dialogName)
        {
            Messenger.Default.Send(new DialogMessage(dialogName, DialogAction.Close));
        }
    }
}
