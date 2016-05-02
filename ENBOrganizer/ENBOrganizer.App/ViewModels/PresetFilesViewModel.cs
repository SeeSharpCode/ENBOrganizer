using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetFilesViewModel : ViewModelBase
    {
        private readonly PresetItemsService _presetItemsService;
        private readonly DialogService _dialogService;

        private Preset _preset;

        public Preset Preset
        {
            get { return _preset; }
            set { Set("Preset", ref _preset, value); }
        }

        public ICommand DeleteItemCommand { get; set; }
        public ICommand AddFileCommand { get; set; }
        public ICommand AddFolderCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand RenameItemCommand { get; set; }

        public IPresetItem SelectedPresetItem { get; set; }

        public List<IPresetItem> Items
        {
            get { return Preset != null ? _presetItemsService.GetPresetItems(Preset.Directory.FullName) : new List<IPresetItem>(); }
        }

        public PresetFilesViewModel(PresetItemsService presetItemsService, DialogService dialogService)
        {
            _presetItemsService = presetItemsService;
            _dialogService = dialogService;

            DeleteItemCommand = new RelayCommand(DeleteItem);
            AddFileCommand = new RelayCommand(AddFile);
            AddFolderCommand = new RelayCommand(AddFolder);
            OpenFileCommand = new RelayCommand(OpenFile);
            RenameItemCommand = new RelayCommand(RenameItem);
        }

        private void DeleteItem()
        {
            SelectedPresetItem.Delete();

            RaisePropertyChanged("Items");
        }

        private void AddFolder()
        {
            string folderPath = _dialogService.PromptForFolder("Please select a folder...");

            if (folderPath == string.Empty)
                return;

            _presetItemsService.CopyDirectoryAsPresetItem(SelectedPresetItem, folderPath);

            // TODO: use service event to react to items change
            RaisePropertyChanged("Items");
        }

        private void AddFile()
        {
            List<string> fileNames = _dialogService.PromptForFiles("Please select a file(s) to add...", "All Files (*.*)|*.*");

            if (!fileNames.Any())
                return;

            foreach (string filePath in fileNames)
                _presetItemsService.CopyFileAsPresetItem(SelectedPresetItem, filePath);

            RaisePropertyChanged("Items");
        }

        private void OpenFile()
        {
            // TODO: exception handling
            Process.Start(SelectedPresetItem.Path);
        }

        private async void RenameItem()
        {
            string newName = await _dialogService.ShowInputDialog("Rename", "Please enter a new name without the file extension.");

            if (newName == null || newName.Trim() == string.Empty)
                return;

            if (OverwriteRequiresRename(newName))
                if (await _dialogService.ShowYesOrNoDialog("Overwrite?", GetOverwritePromptMessage()) == MessageDialogResult.Affirmative)
                    SelectedPresetItem.Rename(newName.Trim());
                else
                    return;
            else
                SelectedPresetItem.Rename(newName.Trim());

            RaisePropertyChanged("Items");
        }

        private string GetOverwritePromptMessage()
        {
            return SelectedPresetItem is PresetFile ? "Overwrite existing file?" : "Overwrite existing directory?";
        }

        private bool OverwriteRequiresRename(string newName)
        {
            string renamedPath = Path.Combine(Path.GetDirectoryName(SelectedPresetItem.Path), newName);

            if (SelectedPresetItem is PresetFile)
                renamedPath += Path.GetExtension(SelectedPresetItem.Path);

            return SelectedPresetItem is PresetFile ? new FileInfo(renamedPath).Exists : new DirectoryInfo(renamedPath).Exists;
        }
    }
}
