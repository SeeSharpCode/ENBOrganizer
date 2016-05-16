using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetsOverviewViewModel : ViewModelBase
    {
        private readonly PresetService _presetService;
        private readonly DialogService _dialogService;

        private readonly ICommand _addBlankPresetCommand;
        private readonly ICommand _importDirectoryOrArchiveCommand;
        private readonly ICommand _importActiveFilesCommand;
        public ObservableCollection<PresetViewModel> PresetViewModels { get; set; }

        public Game CurrentGame { get { return Properties.Settings.Default.CurrentGame; } }
        public ICommand DisableAllPresetsCommand { get; set; }
        public List<TitledCommand> TitledCommands { get; set; }

        public bool CurrentGameHasNoPresets { get { return CurrentGame != null && !PresetViewModels.Any(); } }

        public PresetsOverviewViewModel(PresetService presetService, DialogService dialogService)
        {
            _presetService = presetService;
            _presetService.ItemsChanged += _presetService_ItemsChanged;

            _dialogService = dialogService;

            _addBlankPresetCommand = new RelayCommand(AddBlank, () => CurrentGame != null);
            _importDirectoryOrArchiveCommand = new RelayCommand(() => _dialogService.ShowDialog(Dialog.ImportPreset), () => CurrentGame != null);
            _importActiveFilesCommand = new RelayCommand(ImportActiveFiles, () => CurrentGame != null);

            TitledCommands = new List<TitledCommand>
            {
                new TitledCommand("Blank", "Create a blank preset", _addBlankPresetCommand),
                new TitledCommand("Import Folder or Archive", "Create a preset from a folder or archive file", _importDirectoryOrArchiveCommand),
                new TitledCommand("Import Active Files", "Create a preset from preset files/folders currently in your game folder", _importActiveFilesCommand)
            };

            DisableAllPresetsCommand = new RelayCommand(DisableAllPresets);

            Properties.Settings.Default.PropertyChanged += ApplicationSettings_PropertyChanged;

            LoadPresets();
        }

        private async void DisableAllPresets()
        {
            try
            {
                _presetService.DisableAll(CurrentGame);

                foreach (PresetViewModel presetViewModel in PresetViewModels)
                    presetViewModel.IsEnabled = false;
            }
            catch (Exception exception)
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private void ApplicationSettings_PropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(CurrentGame))
            {
                RaisePropertyChanged(nameof(CurrentGame));
                LoadPresets();
                RaisePropertyChanged(nameof(CurrentGameHasNoPresets));
            }
                
        }

        private void LoadPresets()
        {
            if (PresetViewModels == null)
                PresetViewModels = new ObservableCollection<PresetViewModel>();

            PresetViewModels.Clear();

            PresetViewModels.AddAll(_presetService.GetByGame(CurrentGame).Select(preset => new PresetViewModel(preset)).ToObservableCollection());
        }

        private void _presetService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            Preset preset = repositoryChangedEventArgs.Entity as Preset;

            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                PresetViewModels.Add(new PresetViewModel(preset));
            else
                PresetViewModels.Remove(PresetViewModels.First(presetViewModel => presetViewModel.Preset.Equals(preset)));

            RaisePropertyChanged(nameof(CurrentGameHasNoPresets));
        }

        private async void AddBlank()
        {
            string name = await _dialogService.ShowInputDialog("Add Blank Preset", "Please enter a name for your preset:");

            if (string.IsNullOrWhiteSpace(name))
                return;

            try
            {
                _presetService.Add(new Preset(name, CurrentGame));
            }
            catch (Exception exception) 
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private async void ImportActiveFiles()
        {
            string name = await _dialogService.ShowInputDialog("Import Active Files", "Please enter a name for your preset:");

            if (string.IsNullOrWhiteSpace(name))
                return;

            try
            {
                _presetService.ImportActiveFiles(new Preset(name, CurrentGame));
            }
            catch (Exception exception)
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
        }
    }
}
