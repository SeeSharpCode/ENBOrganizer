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

        public string Name { get; set; }
        public string SourcePath { get; set; }
        public Binary Binary { get; set; }
        public Game CurrentGame { get { return Properties.Settings.Default.CurrentGame; } }
        public List<TitledCommand> TitledCommands { get; set; }
        public ObservableCollection<Binary> Binaries { get; set; }

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

            Binaries = _binaryService.GetAll().ToObservableCollection();
        }

        private void _binaryService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                Binaries.Add(repositoryChangedEventArgs.Entity as Binary);
            else
                Binaries.Remove(repositoryChangedEventArgs.Entity as Binary);
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
