using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using System;
using System.Linq;

namespace ENBOrganizer.App.ViewModels.Presets
{
    public class PresetsViewModel : FileSystemViewModel<Preset>
    {
        protected new PresetService DataService { get { return (PresetService)base.DataService; } }
        protected override DialogName DialogName { get { return DialogName.AddPreset; } }
        protected override string DialogHostName { get { return "PresetNameDialog"; } }

        public ICommand ImportInstalledFilesCommand { get; set; }
        public ICommand SyncEnabledPresetsCommand { get; set; }
        public ICommand ChangeImageCommand { get; set; }
        public ICommand ClearImageCommand { get; set; }
        public ICommand AddGlobalEnbLocalCommand { get; set; }
        public ICommand SwitchGlobalENBLocalCommand { get; set; }
        public ICommand OpenAddENBoostPresetCommand { get; set; }

        public PresetsViewModel(PresetService presetService)
            : base(presetService)
        {
            ImportInstalledFilesCommand = new RelayCommand(ImportInstalledFiles, CanAdd);
            SyncEnabledPresetsCommand = new RelayCommand(SyncEnabledPresets, () => Models.Any(preset => preset.IsEnabled));
            ChangeImageCommand = new RelayCommand<Preset>(ChangeImage);
            ClearImageCommand = new RelayCommand<Preset>(ClearImage);
            AddGlobalEnbLocalCommand = new RelayCommand(() => _dialogService.ShowDialog(DialogName.GlobalEnbLocal), CanAdd);
            SwitchGlobalENBLocalCommand = new RelayCommand<Preset>(SwitchGlobalENBLocal);
            OpenAddENBoostPresetCommand = new RelayCommand(() => _dialogService.ShowDialog(DialogName.AddENBoostPreset));
        }

        private void SwitchGlobalENBLocal(Preset preset)
        {
            preset.IsGlobalENBLocalEnabled = !preset.IsGlobalENBLocalEnabled;

            DataService.SaveChanges();

            try
            {
                // Re-enable the preset so that the global enblocal change takes effect.
                if (preset.IsEnabled)
                    preset.Enable();
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog("Your preset is now set up to use the global enblocal.ini file, but there was an " +
                    "error trying to re-enable the preset. You may need to disable and then enable the preset for changes to take effect."
                    + Environment.NewLine + Environment.NewLine + exception.Message);
            }
        }

        private void SyncEnabledPresets()
        {
            try
            {
                DataService.SyncEnabledPresets();

                _dialogService.ShowInfoDialog("Preset(s) refreshed successfully.");
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog("Error syncing presets." + Environment.NewLine + exception.Message);
            }
        }

        private void ClearImage(Preset preset)
        {
            preset.ImagePath = null;

            DataService.SaveChanges();
        }

        protected override void Edit(Preset entity)
        {
            _dialogService.ShowDialog(DialogName.EditPreset);
            MessengerInstance.Send(entity);
        }

        private void ChangeImage(Preset preset)
        {
            string imagePath = _dialogService.ShowOpenFileDialog("Please select an image file", "All Files (*.*)|*.*");

            if (!string.IsNullOrWhiteSpace(imagePath))
                preset.ImagePath = imagePath;

            DataService.SaveChanges();
        }

        private async void ImportInstalledFiles()
        {
            string name = ((string)await _dialogService.ShowInputDialog("Please enter a name for the preset:", "PresetNameDialog")).Trim();

            try
            {
                DataService.ImportInstalledFiles(new Preset(name, SettingsService.CurrentGame) { IsEnabled = true });
            }
            catch (DuplicateEntityException)
            {
                _dialogService.ShowErrorDialog("A preset named " + name + " already exists.");
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog("Error adding preset." + Environment.NewLine + exception.Message);
            }
        }
    }
}