using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
        private readonly PresetDetailViewModel _presetDetailViewModel;
        private readonly DialogService _dialogService;

        private readonly ICommand _addBlankPresetCommand;
        private readonly ICommand _importFolderCommand;
        private readonly ICommand _importArchiveCommand;
        private readonly ICommand _importActiveFilesCommand;
        public ObservableCollection<PresetViewModel> PresetViewModels { get; set; }

        public Game CurrentGame { get { return Properties.Settings.Default.CurrentGame; } }
        public ICommand SelectPresetCommand { get; private set; }
        public ICommand DisablePresetCommand { get; set; }
        public ICommand EnablePresetCommand { get; set; }
        public ICommand ChangePresetImageCommand { get; set; }
        public ICommand DisableAllPresetsCommand { get; set; }
        public List<TitledCommand> TitledCommands { get; set; }

        public PresetsOverviewViewModel(PresetService presetService, PresetDetailViewModel presetDetailViewModel, DialogService dialogService)
        {
            _presetService = presetService;
            _presetService.ItemsChanged += _presetService_ItemsChanged;

            _dialogService = dialogService;

            _presetDetailViewModel = presetDetailViewModel;

            _addBlankPresetCommand = new RelayCommand(AddBlank, () => true);
            _importFolderCommand = new RelayCommand(ImportDirectory, () => true);
            _importArchiveCommand = new RelayCommand(ImportArchive, () => true);
            _importActiveFilesCommand = new RelayCommand(ImportActiveFiles, () => true);

            TitledCommands = new List<TitledCommand>
            {
                new TitledCommand("Blank", "Create a blank preset", _addBlankPresetCommand),
                new TitledCommand("Import Folder", "Create a preset from a folder", _importFolderCommand),
                new TitledCommand("Import Archive", "Create a preset an archive (.zip, .7z)", _importArchiveCommand),
                new TitledCommand("Import Active Files", "Create a preset from preset files/folders currently in your game folder", _importActiveFilesCommand)
            };

            SelectPresetCommand = new RelayCommand<Preset>(preset => MessengerInstance.Send(new PresetNavigationMessage(preset)));
            EnablePresetCommand = new RelayCommand<Preset>(preset => _presetService.Enable(preset));
            DisablePresetCommand = new RelayCommand<Preset>(preset => _presetService.Disable(preset));
            ChangePresetImageCommand = new RelayCommand<Preset>(ChangePresetImage);
            DisableAllPresetsCommand = new RelayCommand(() => _presetService.DisableAll(CurrentGame));

            Properties.Settings.Default.PropertyChanged += ApplicationSettings_PropertyChanged;

            LoadPresets();
        }

        private void ChangePresetImage(Preset preset)
        {
            // TODO: filter
            string imageSource = _dialogService.PromptForFile("Select an image", "All Files (*.*)|*.*");
            
            if (string.IsNullOrWhiteSpace(imageSource))
                return;

            preset.ImagePath = imageSource;
            _presetService.SaveChanges();
        }

        private void ApplicationSettings_PropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "CurrentGame")
                LoadPresets();
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
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                PresetViewModels.Add(new PresetViewModel(repositoryChangedEventArgs.Entity as Preset));
            else
                PresetViewModels.Remove(PresetViewModels.First(presetViewModel => presetViewModel.Preset.Equals(repositoryChangedEventArgs.Entity as Preset)));
        }

        private async void AddBlank()
        {
            // TODO: exception handling
            string name = await _dialogService.ShowInputDialog("Add Blank Preset", "Please enter a name for your preset:");

            if (string.IsNullOrWhiteSpace(name))
                return;

            try
            {
                _presetService.Add(new Preset(name, CurrentGame));
            }
            catch (DuplicateEntityException exception)
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private async void ImportActiveFiles()
        {
            // TODO: exception handling
            string name = await _dialogService.ShowInputDialog("Import Active Files", "Please enter a name for your preset:");

            _presetService.ImportActiveFiles(new Preset(name, CurrentGame));
        }

        private void ImportArchive()
        {
            string archivePath = _dialogService.PromptForFile("Please select a .zip file", "ZIP Files(*.zip) | *.zip");

            if (string.IsNullOrWhiteSpace(archivePath))
                return;

            _presetService.ImportArchive(archivePath, CurrentGame);
        }

        private void ImportDirectory()
        {
            string directoryPath = _dialogService.PromptForFolder("Please select the preset folder...");

            if (string.IsNullOrWhiteSpace(directoryPath))
                return;

            _presetService.ImportDirectory(directoryPath, CurrentGame);
        }
    }
}
