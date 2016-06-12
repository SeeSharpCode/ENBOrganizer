using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels.Presets
{
    public class PresetsViewModel : FileSystemViewModel<Preset>
    {
        protected new PresetService DataService { get { return (PresetService)base.DataService; } }
        protected override DialogName DialogName { get { return DialogName.AddPreset; } }
        protected override string DialogHostName { get { return "PresetNameDialog"; } }

        public ICommand ImportActiveFilesCommand { get; set; }
        public ICommand ChangeImageCommand { get; set; }
        public ICommand ClearImageCommand { get; set; }

        public PresetsViewModel(PresetService presetService)
            : base(presetService)
        {
            ImportActiveFilesCommand = new RelayCommand(ImportActiveFiles, CanAdd);
            ChangeImageCommand = new RelayCommand<Preset>(ChangeImage);
            ClearImageCommand = new RelayCommand<Preset>(ClearImage);
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

        private async void ImportActiveFiles()
        {
            string name = ((string)await _dialogService.ShowInputDialog("Please enter a name for the preset:", "PresetNameDialog")).Trim();

            try
            {
                DataService.ImportActiveFiles(new Preset(name, SettingsService.CurrentGame));
            }
            catch (DuplicateEntityException) // TODO: double check these
            {
                _dialogService.ShowErrorDialog("A preset named " + name + " already exists.");
            }
            
        }
    }
}
