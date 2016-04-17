using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetDetailViewModel : ViewModelBase
    {
        private readonly PresetItemsService _presetItemsService;

        private Preset _preset;

        public ICommand DeleteItemCommand { get; set; }
        public ICommand AddFileCommand { get; set; }
        public ICommand AddFolderCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand RenameItemCommand { get; set; }
        public IPresetItem SelectedPresetItem { get; set; }

        public Preset Preset
        {
            get { return _preset; }
            set
            {
                _preset = value;
                RaisePropertyChanged("Preset");
                RaisePropertyChanged("Items");
            }
        }

        public List<IPresetItem> Items
        {
            get { return _presetItemsService.GetPresetItems(Path.Combine(Preset.Game.PresetsDirectory.FullName, Preset.Name)); }
        }

        public PresetDetailViewModel(PresetItemsService presetItemsService)
        {
            _presetItemsService = presetItemsService;

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
            string folderPath = DialogService.PromptForFolder("Please select a folder...");

            if (folderPath == string.Empty)
                return;

            _presetItemsService.CopyDirectoryAsPresetItem(SelectedPresetItem, folderPath);

            // TODO: use service event to react to items change
            RaisePropertyChanged("Items");
        }

        private void AddFile()
        {
            List<string> fileNames = DialogService.PromptForFiles("Please select a file(s) to add...", "All Files (*.*)|*.*");

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
            string newName = await DialogService.ShowInputDialog("Rename", "Please select a new name");

            if (newName.Trim() == string.Empty)
                return;

            SelectedPresetItem.Rename(newName);

            RaisePropertyChanged("Items");
        }
    }
}
