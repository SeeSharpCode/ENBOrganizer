using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetDetailViewModel : ViewModelBase
    {
        private readonly PresetService _presetService;
        private readonly DialogService _dialogService;
        private readonly PresetFilesViewModel _presetFilesViewModel;

        private Preset _preset;
        
        public ICommand RenamePresetCommand { get; set; }
        public ICommand NavigateToPresetOverviewCommand { get; set; }
        public ICommand DeletePresetCommand { get; set; }

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

        public PresetDetailViewModel(PresetService presetServce, DialogService dialogService, PresetFilesViewModel presetFilesViewModel)
        {
            _presetService = presetServce;
            _dialogService = dialogService;
            _presetFilesViewModel = presetFilesViewModel;
            
            NavigateToPresetOverviewCommand = new RelayCommand(NavigateToPresetsOverview);
            DeletePresetCommand = new RelayCommand(DeletePreset);
            
            RenamePresetCommand = new RelayCommand(RenamePreset);

            MessengerInstance.Register<PresetNavigationMessage>(this, OnPresetSelected);
        }

        // TODO: is there a better way to do this?
        private void OnPresetSelected(PresetNavigationMessage message)
        {
            _preset = message.Preset;
            _presetFilesViewModel.Preset = message.Preset;
        }

        private async void RenamePreset()
        {
            string newName = await _dialogService.ShowInputDialog("Rename Preset", "Please enter a new name for the preset.");

            if (newName == null || newName.Trim() == string.Empty)
                return;

            _preset.Directory.Rename(newName);

            Name = newName;
            _presetService.SaveChanges();
        }

        private void NavigateToPresetsOverview()
        {
            MessengerInstance.Send(new NavigationMessage(ViewName.PresetsOverview));
        }

        private void DeletePreset()
        {
            _presetService.Delete(_preset);

            NavigateToPresetsOverview();
        }
    }
}