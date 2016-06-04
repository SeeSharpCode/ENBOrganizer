using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetsViewModel : FileSystemViewModel<Preset>
    {
        protected new PresetService DataService { get { return (PresetService)base.DataService; } }
        protected override DialogName DialogName { get { return DialogName.AddPreset; } }
        protected override string DialogHostName { get { return "PresetNameDialog"; } }

        public ICommand ImportActiveFilesCommand { get; set; }
        public ICommand ChangeImageCommand { get; set; }

        public PresetsViewModel(PresetService presetService)
            : base(presetService)
        {            
            ImportActiveFilesCommand = new RelayCommand(ImportActiveFiles, () => CurrentGame != null);
            ChangeImageCommand = new RelayCommand<Preset>(ChangeImage);
        }

        protected override void Edit(Preset entity)
        {
            throw new NotImplementedException();
        }

        private void ChangeImage(Preset preset)
        {
            throw new NotImplementedException();
        }

        private async void ImportActiveFiles()
        {
            string name = (string)await _dialogService.ShowInputDialog("Please enter a name for the preset:", "PresetNameDialog");

            try
            {
                DataService.ImportActiveFiles(new Preset(name, CurrentGame));
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }
    }
}
