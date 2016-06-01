using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetViewModel : ViewModelBase
    {
        private readonly DialogService _dialogService;
        private readonly PresetService _presetService;

        public Preset Preset { get; set; }
        public ICommand ChangeImageCommand { get; set; }
        public ICommand DisablePresetCommand { get; set; }
        public ICommand EnablePresetCommand { get; set; }
        public ICommand RenamePresetCommand { get; set; }
        public ICommand DeletePresetCommand { get; set; }
        public ICommand OpenFileExplorerCommand { get; set; }

        public string Name
        {
            get { return Preset.Name; }
            set
            {
                Preset.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string ImagePath
        {
            get { return Preset.ImagePath; }
            set
            {
                Preset.ImagePath = value;
                RaisePropertyChanged(nameof(ImagePath));
            }
        }

        public bool IsEnabled
        {
            get { return Preset.IsEnabled; }
            set
            {
                Preset.IsEnabled = value;
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        public PresetViewModel(Preset preset)
            : this(preset, SimpleIoc.Default.GetInstance<DialogService>(), SimpleIoc.Default.GetInstance<PresetService>()) { }


        private PresetViewModel(Preset preset, DialogService dialogService, PresetService presetService)
        {
            Preset = preset;
            _dialogService = dialogService;
            _presetService = presetService;

            ChangeImageCommand = new RelayCommand(ChangeImage);
            EnablePresetCommand = new RelayCommand(Enable);
            DisablePresetCommand = new RelayCommand(Disable);
            RenamePresetCommand = new RelayCommand(Rename);
            OpenFileExplorerCommand = new RelayCommand(() => Process.Start(Preset.Directory.FullName));
            DeletePresetCommand = new RelayCommand(() => _presetService.Delete(Preset));
        }

        private void Rename()
        {
            // TODO: string newName = _dialogService.ShowInputDialog("Rename Preset", "Please enter a new name for the preset.");
            string newName = string.Empty;

            if (string.IsNullOrWhiteSpace(newName))
                return;

            try
            {
                Preset.Directory.Rename(newName);

                Name = newName;
                _presetService.SaveChanges();
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private void Enable()
        {
            try
            {
                _presetService.Enable(Preset);

                IsEnabled = true;

                _presetService.SaveChanges();
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private void Disable()
        {
            try
            {
                _presetService.Disable(Preset);

                IsEnabled = false;

                _presetService.SaveChanges();
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private void ChangeImage()
        {
            string imageSource = _dialogService.ShowOpenFileDialog("Select an image", "All Files (*.*)|*.*");

            if (string.IsNullOrWhiteSpace(imageSource))
                return;

            // Ensure the path points to a real image file.
            try
            {
                new BitmapImage(new Uri(imageSource));

                ImagePath = imageSource;
            }
            catch (Exception)
            {
                _dialogService.ShowErrorDialog("The selected file is not a valid image.");

                ImagePath = null;
            }

            _presetService.SaveChanges();
        }
    }
}
