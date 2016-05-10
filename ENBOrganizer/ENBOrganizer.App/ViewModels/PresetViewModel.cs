using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System.Windows.Input;
using System;
using ENBOrganizer.Util.IO;
using System.Diagnostics;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetViewModel : ViewModelBase
    {
        private Preset _preset;
        private readonly DialogService _dialogService;
        private readonly PresetService _presetService;

        public ICommand ChangeImageCommand { get; set; }
        public ICommand DisablePresetCommand { get; set; }
        public ICommand EnablePresetCommand { get; set; }
        public ICommand RenamePresetCommand { get; set; }
        public ICommand DeletePresetCommand { get; set; }
        public ICommand OpenFileExplorerCommand { get; set; }

        public string Name
        {
            get { return _preset.Name; }
            set
            {
                _preset.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string ImagePath
        {
            get { return _preset.ImagePath; }
            set
            {
                _preset.ImagePath = value;
                RaisePropertyChanged("ImagePath");
            }
        }

        public bool IsEnabled
        {
            get { return _preset.IsEnabled; }
            set
            {
                _preset.IsEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }

        public PresetViewModel(Preset preset)
            : this(preset, SimpleIoc.Default.GetInstance<DialogService>(), SimpleIoc.Default.GetInstance<PresetService>()) { }


        private PresetViewModel(Preset preset, DialogService dialogService, PresetService presetService)
        {
            _preset = preset;
            _dialogService = dialogService;
            _presetService = presetService;

            ChangeImageCommand = new RelayCommand(ChangeImage);
            EnablePresetCommand = new RelayCommand(Enable);
            DisablePresetCommand = new RelayCommand(Disable);
            RenamePresetCommand = new RelayCommand(Rename);
            OpenFileExplorerCommand = new RelayCommand(() => Process.Start(_preset.Directory.FullName));
            DeletePresetCommand = new RelayCommand(() => _presetService.Delete(_preset));
        }

        private async void Rename()
        {
            string newName = await _dialogService.ShowInputDialog("Rename Preset", "Please enter a new name for the preset.");

            if (string.IsNullOrWhiteSpace(newName))
                return;

            _preset.Directory.Rename(newName);

            Name = newName;
            _presetService.SaveChanges();
        }

        private void Enable()
        {
            _presetService.Enable(_preset);

            IsEnabled = true;

            _presetService.SaveChanges();
        }

        private void Disable()
        {
            _presetService.Disable(_preset);

            IsEnabled = false;

            _presetService.SaveChanges();
        }

        public Preset GetPreset()
        {
            return _preset;
        }

        private void ChangeImage()
        {
            // TODO: filter
            string imageSource = _dialogService.PromptForFile("Select an image", "All Files (*.*)|*.*");

            if (string.IsNullOrWhiteSpace(imageSource))
                return;

            ImagePath = imageSource;
            _presetService.SaveChanges();
        }
    }
}
