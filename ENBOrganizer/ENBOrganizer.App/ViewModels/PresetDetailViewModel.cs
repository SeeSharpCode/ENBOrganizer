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

namespace ENBOrganizer.App.ViewModels
{
    public class PresetDetailViewModel : ViewModelBase
    {
        private readonly PresetService _presetService;
        private readonly PresetItemsService _presetItemsService;

        private Preset _preset;

        public ICommand ChangePresetImageCommand { get; set; }
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

        public string Name { get { return _preset.Name; } }

        public List<IPresetItem> Items
        {
            get { return _presetItemsService.GetPresetItems(Path.Combine(_preset.Game.PresetsDirectory.FullName, _preset.Name)); }
        }

        public PresetDetailViewModel(PresetService presetServce, PresetItemsService presetItemsService)
        {
            _presetService = presetServce;
            _presetItemsService = presetItemsService;

            ChangePresetImageCommand = new RelayCommand(ChangePresetImage);
            NavigateToPresetOverviewCommand = new RelayCommand(NavigateToPresetsOverview);
            DeletePresetCommand = new RelayCommand(DeletePreset);
            DeleteItemCommand = new RelayCommand(DeleteItem);
            AddFileCommand = new RelayCommand(AddFile);
            AddFolderCommand = new RelayCommand(AddFolder);
            OpenFileCommand = new RelayCommand(OpenFile);
            RenameItemCommand = new RelayCommand(RenameItem);

            MessengerInstance.Register<PresetNavigationMessage>(this, (message) => _preset = message.Preset);
        }

        private void NavigateToPresetsOverview()
        {
            MessengerInstance.Send(new NavigationMessage(ViewNames.PresetsOverview));
        }

        private void ChangePresetImage()
        {
            // TODO: filter
            string imageSource = DialogService.PromptForFile("Select an image", "All Files (*.*)|*.*");

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

            if (newName == null || newName.Trim() == string.Empty)
                return;

            try
            {
                SelectedPresetItem.Rename(newName.Trim());

                RaisePropertyChanged("Items");
            }
            catch (IOException exception)
            {
                await DialogService.ShowErrorDialog(exception.Message);
            }
            
        }
    }
}
