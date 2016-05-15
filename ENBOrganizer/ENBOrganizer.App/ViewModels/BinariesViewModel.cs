using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class BinariesViewModel : ViewModelBase
    {
        private readonly BinaryService _binaryService;
        private readonly DialogService _dialogService;

        private ICommand _importDirectoryCommand;
        private ICommand _importArchiveCommand;
        private ICommand _importActiveFilesCommand;

        public List<TitledCommand> TitledCommands { get; set; }
        public Game CurrentGame { get { return Properties.Settings.Default.CurrentGame; } }
        public ICommand OpenFilesCommand { get; set; }
        public ICommand DeleteBinaryCommand { get; set; }
        public ObservableCollection<Binary> Binaries { get; set; }

        public BinariesViewModel(BinaryService binaryService, DialogService dialogService)
        {
            _binaryService = binaryService;
            _binaryService.ItemsChanged += _binaryService_ItemsChanged;

            _dialogService = dialogService;

            _importDirectoryCommand = new RelayCommand(ImportDirectory, () => CurrentGame != null);
            _importArchiveCommand = new RelayCommand(ImportArchive, () => CurrentGame != null);
            _importActiveFilesCommand = new RelayCommand(ImportActiveFiles, () => CurrentGame != null);

            TitledCommands = new List<TitledCommand>
            {
                new TitledCommand("Import Folder", "Create from a folder", _importDirectoryCommand),
                new TitledCommand("Import Archive", "Create from an archive (.zip, .7z)", _importArchiveCommand),
                new TitledCommand("Import Active Files", "Create from binary files currently in your game folder", _importActiveFilesCommand)
            };

            OpenFilesCommand = new RelayCommand<Binary>(binary => Process.Start(binary.Directory.FullName));
            DeleteBinaryCommand = new RelayCommand<Binary>(binary => _binaryService.Delete(binary));

            Binaries = _binaryService.GetAll().ToObservableCollection();
        }

        // TODO: this logic is repeated in most ViewModels
        private void _binaryService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                Binaries.Add(repositoryChangedEventArgs.Entity as Binary);
            else
                Binaries.Remove(repositoryChangedEventArgs.Entity as Binary);
        }

        private async void ImportDirectory()
        {
            string directoryPath = _dialogService.PromptForFolder("Please select the binary folder...");

            if (string.IsNullOrWhiteSpace(directoryPath))
                return;

            try
            {
                _binaryService.ImportDirectory(directoryPath, CurrentGame);
            }
            catch (Exception exception)
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private async void ImportArchive()
        {
            // TODO: this should support more than just .zip
            string archivePath = _dialogService.PromptForFile("Please select an archive file", "ZIP Files(*.zip) | *.zip");

            if (string.IsNullOrWhiteSpace(archivePath))
                return;

            try
            {
                _binaryService.ImportArchive(archivePath, CurrentGame);
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
                _binaryService.ImportActiveFiles(new Binary(name, CurrentGame));
            }
            catch (Exception exception)
            {
                await _dialogService.ShowErrorDialog(exception.Message);
            }
        }
    }
}
