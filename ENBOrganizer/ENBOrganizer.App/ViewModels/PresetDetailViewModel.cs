using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Domain.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using System;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetDetailViewModel : ViewModelBase
    {
        private readonly PresetService _presetService;
        private readonly PresetItemsService _presetItemsService;
        private readonly DialogService _dialogService;

        private Preset _preset;

        public ICommand ChangePresetImageCommand { get; set; }
        public ICommand RenamePresetCommand { get; set; }
        public ICommand NavigateToPresetOverviewCommand { get; set; }
        public ICommand DeletePresetCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        public ICommand AddFileCommand { get; set; }
        public ICommand AddFolderCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand RenameItemCommand { get; set; }
        public IPresetItem SelectedPresetItem { get; set; }

        public string ImagePath
        {
            get { return _preset.ImagePath; }
            set
            {
                _preset.ImagePath = value;
                RaisePropertyChanged("ImagePath");
            }
        }

        public string Name
        {
            get { return _preset.Name; }
            set
            {
                _preset.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public List<IPresetItem> Items
        {
            get { return _presetItemsService.GetPresetItems(Path.Combine(_preset.Game.PresetsDirectory.FullName, _preset.Name)); }
        }

        public PresetDetailViewModel(PresetService presetServce, PresetItemsService presetItemsService, DialogService dialogService)
        {
            _presetService = presetServce;
            _presetItemsService = presetItemsService;

            _dialogService = dialogService;

            ChangePresetImageCommand = new RelayCommand(ChangePresetImage);
            NavigateToPresetOverviewCommand = new RelayCommand(NavigateToPresetsOverview);
            DeletePresetCommand = new RelayCommand(DeletePreset);
            DeleteItemCommand = new RelayCommand(DeleteItem);
            AddFileCommand = new RelayCommand(AddFile);
            AddFolderCommand = new RelayCommand(AddFolder);
            OpenFileCommand = new RelayCommand(OpenFile);
            RenameItemCommand = new RelayCommand(RenameItem);
            RenamePresetCommand = new RelayCommand(RenamePreset);

            MessengerInstance.Register<PresetNavigationMessage>(this, (message) => _preset = message.Preset);
        }

        private void RenamePreset()
        {
            Name = "Renamed Name";
            _presetService.Save();
        }

        private void NavigateToPresetsOverview()
        {
            MessengerInstance.Send(new NavigationMessage(ViewNames.PresetsOverview));
        }

        private void ChangePresetImage()
        {
            // TODO: filter
            string imageSource = _dialogService.PromptForFile("Select an image", "All Files (*.*)|*.*");

            ImagePath = imageSource;

            _presetService.ChangeImage(_preset, imageSource);
        }

        private void DeletePreset()
        {
            _presetService.Delete(_preset);

            NavigateToPresetsOverview();
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
