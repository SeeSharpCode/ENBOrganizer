using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System;
using System.Collections.ObjectModel;
using ENBOrganizer.Domain;
using ENBOrganizer.Util;

namespace ENBOrganizer.App.ViewModels
{
    public class ImportPresetViewModel : ViewModelBase
    {
        private readonly PresetService _presetService;
        private readonly BinaryService _binaryService;
        private readonly DialogService _dialogService;

        private ICommand _browseForDirectoryCommand;
        private ICommand _browseForArchiveCommand;
        
        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(nameof(Name), ref _name, value.Trim()); }
        }

        private string _sourcePath;

        public string SourcePath
        {
            get { return _sourcePath; }
            set { Set(nameof(SourcePath), ref _sourcePath, value.Trim()); }
        }

        public Binary Binary { get; set; }
        public Game CurrentGame { get { return Properties.Settings.Default.CurrentGame; } }
        public List<TitledCommand> TitledCommands { get; set; }
        public ObservableCollection<Binary> Binaries { get; set; }
        public ICommand SaveCommand { get; set; }

        public ImportPresetViewModel(PresetService presetService, BinaryService binaryService, DialogService dialogService)
        {
            _presetService = presetService;
            _binaryService = binaryService;
            _binaryService.ItemsChanged += _binaryService_ItemsChanged;

            _dialogService = dialogService;

            _browseForDirectoryCommand = new RelayCommand(BrowseForDirectory);
            _browseForArchiveCommand = new RelayCommand(BrowseForArchive);

            TitledCommands = new List<TitledCommand>
            {
                new TitledCommand("Browse Folder", "Browse for a folder", _browseForDirectoryCommand),
                new TitledCommand("Browse Archive", "Browse for an archive file", _browseForArchiveCommand)
            };

            SaveCommand = new RelayCommand(Save, CanSave);

            Binaries = _binaryService.GetAll().ToObservableCollection();
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(SourcePath)
                && (Directory.Exists(SourcePath) || File.Exists(SourcePath));
        }

        private void _binaryService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                Binaries.Add(repositoryChangedEventArgs.Entity as Binary);
            else
                Binaries.Remove(repositoryChangedEventArgs.Entity as Binary);
        }

        private void Save()
        {
            Preset preset = new Preset(Name, CurrentGame);

            // Detect whether the user has selected the default value in the ComboBox.
            if (Binary.Name != "-- None --" && Binary.Game != null)
                preset.Binary = Binary;

            try
            {
                _presetService.Import(preset, SourcePath);
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
            finally
            {
                Name = string.Empty;
                SourcePath = string.Empty;

                //_dialogService.CloseDialog();
            }
        }

        private void BrowseForArchive()
        {
            // TODO: more than .zip
            string archivePath = _dialogService.PromptForFile("Please select an archive file", "ZIP Files(*.zip) | *.zip");

            if (string.IsNullOrWhiteSpace(archivePath))
                return;

            FileInfo file = new FileInfo(archivePath);

            Name = file.Name;
            SourcePath = file.FullName;
        }

        private void BrowseForDirectory()
        {
            string directoryPath = _dialogService.PromptForFolder("Please select the preset folder");

            if (string.IsNullOrWhiteSpace(directoryPath))
                return;

            DirectoryInfo directory = new DirectoryInfo(directoryPath);

            Name = directory.Name;
            SourcePath = directory.FullName;
        }
    }
}
