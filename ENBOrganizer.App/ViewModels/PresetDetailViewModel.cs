using ENBOrganizer.App.Messages;
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
        private readonly PresetService _presetService;
        private readonly PresetItemsService _presetItemsService;

        private Preset _preset;

        public ICommand ChangePresetImageCommand { get; set; }
        public ICommand GoToPresetsOverviewCommand { get; set; }
        public ICommand DeletePresetCommand { get; set; }
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

        public string ImagePath
        {
            get { return Preset.ImagePath; }
            set
            {
                Preset.ImagePath = value;
                RaisePropertyChanged("ImagePath");
            }
        }

        public List<IPresetItem> Items
        {
            get { return _presetItemsService.GetPresetItems(Path.Combine(Preset.Game.PresetsDirectory.FullName, Preset.Name)); }
        }

        public PresetDetailViewModel(PresetService presetServce, PresetItemsService presetItemsService)
        {
            _presetService = presetServce;
            _presetItemsService = presetItemsService;

            ChangePresetImageCommand = new RelayCommand(ChangePresetImage);
            GoToPresetsOverviewCommand = new RelayCommand(NavigateToPresetsOverview);
            DeletePresetCommand = new RelayCommand(DeletePreset);
            DeleteItemCommand = new RelayCommand(DeleteItem);
            AddFileCommand = new RelayCommand(AddFile);
            AddFolderCommand = new RelayCommand(AddFolder);
            OpenFileCommand = new RelayCommand(OpenFile);
            RenameItemCommand = new RelayCommand(RenameItem);
        }

        private void NavigateToPresetsOverview()
        {
            MessengerInstance.Send(new NavigationMessage(ViewNames.PresetsOverview));
        }

        private void ChangePresetImage()
        {
            // TODO: filter
            string imageSource = DialogService.PromptForFile("Select an image", "All Files (*.*)|*.*");

            _presetService.ChangeImage(Preset, imageSource);

            RaisePropertyChanged("Preset");
        }

        private void DeletePreset()
        {
            _presetService.Delete(Preset);

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

            if (newName.Trim() == string.Empty)
                return;

            try
            {
                SelectedPresetItem.Rename(newName);

                RaisePropertyChanged("Items");
            }
            catch (IOException exception)
            {
                await DialogService.ShowErrorDialog(exception.Message);
            }
            
        }
    }
}
