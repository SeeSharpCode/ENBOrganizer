using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetsViewModel : PageViewModelBase<Preset>
    {
        protected new PresetService DataService { get { return (PresetService)base.DataService; } }
        private readonly PresetService _presetService;
        protected override DialogName DialogName { get { return DialogName.AddPreset; } }
        
        public Game CurrentGame { get { return Properties.Settings.Default.CurrentGame; } }
        public ICommand OpenAddPresetDialogCommand { get; set; }
        public ICommand DisableAllPresetsCommand { get; set; }
        public bool CurrentGameHasNoPresets { get { return CurrentGame != null && !PresetViewModels.Any(); } }

        private bool _isAddPresetDialogOpen;

        public bool IsAddPresetDialogOpen
        {
            get { return _isAddPresetDialogOpen; }
            set { Set(nameof(IsAddPresetDialogOpen), ref _isAddPresetDialogOpen, value); }
        }

        public PresetsViewModel(PresetService presetService, DialogService dialogService)
        {
            _presetService = presetService;
            _presetService.ItemsChanged += _presetService_ItemsChanged;

            _dialogService = dialogService;
            
            _importActiveFilesCommand = new RelayCommand(ImportActiveFiles, () => CurrentGame != null);
            OpenAddPresetDialogCommand = new RelayCommand(() => _dialogService.ShowDialog(DialogName.AddPreset), () => CurrentGame != null);
            DisableAllPresetsCommand = new RelayCommand(DisableAllPresets);

            Properties.Settings.Default.PropertyChanged += ApplicationSettings_PropertyChanged;

            MessengerInstance.Register<DialogMessage>(this, OnDialogMessageReceived);

            LoadPresets();
        }

        private void OnDialogMessageReceived(DialogMessage message)
        {
            if (message.DialogName != DialogName.AddPreset)
                return;

            IsAddPresetDialogOpen = message.DialogAction == DialogAction.Open;
        }

        private void DisableAll()
        {
            try
            {
                DataService.DisableAll(CurrentGame);
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
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

        private async void ImportActiveFiles()
        {
            //string name = await _dialogService.ShowInputDialog("Import Active Files", "Please enter a name for your preset:");

            //if (string.IsNullOrWhiteSpace(name))
            //    return;

            //try
            //{
            //    _presetService.ImportActiveFiles(new Preset(name, CurrentGame));
            //}
            //catch (Exception exception)
            //{
            //    _dialogService.ShowErrorDialog(exception.Message);
            //}
        }
    }
}
