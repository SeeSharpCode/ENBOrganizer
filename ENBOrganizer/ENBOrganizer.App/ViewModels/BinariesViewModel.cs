using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class BinariesViewModel : ViewModelBase, IPageViewModel
    {
        private readonly BinaryService _binaryService;
        private readonly DialogService _dialogService;
        private readonly InputDialogViewModel _inputDialogViewModel;

        public ICommand ImportDirectoryCommand { get; set; }
        public ICommand ImportArchiveCommand { get; set; }
        public ICommand OpenFilesCommand { get; set; }
        public ICommand DeleteBinaryCommand { get; set; }
        public string Name { get { return "Binaries"; } }
        public Game CurrentGame { get { return Properties.Settings.Default.CurrentGame; } }
        public ObservableCollection<Binary> Binaries { get; set; }

        private bool _isInputDialogOpen;

        public bool IsInputDialogOpen
        {
            get { return _isInputDialogOpen; }
            set { Set(nameof(IsInputDialogOpen), ref _isInputDialogOpen, value); }
        }
        
        public BinariesViewModel(BinaryService binaryService, DialogService dialogService, InputDialogViewModel inputDialogViewModel)
        {
            _binaryService = binaryService;
            _binaryService.ItemsChanged += _binaryService_ItemsChanged;

            _dialogService = dialogService;
            _inputDialogViewModel = inputDialogViewModel;

            ImportDirectoryCommand = new RelayCommand(ImportDirectory, () => CurrentGame != null);
            ImportArchiveCommand = new RelayCommand(ImportArchive, () => CurrentGame != null);
            OpenFilesCommand = new RelayCommand<Binary>(binary => Process.Start(binary.Directory.FullName));
            DeleteBinaryCommand = new RelayCommand<Binary>(binary => _binaryService.Delete(binary));

            Binaries = _binaryService.GetAll().ToObservableCollection();
        }
        
        private void _binaryService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                Binaries.Add(repositoryChangedEventArgs.Entity as Binary);
            else
                Binaries.Remove(repositoryChangedEventArgs.Entity as Binary);
        }

        private void ImportDirectory()
        {
            string directoryPath = _dialogService.PromptForFolder("Please select the binary folder...");

            if (string.IsNullOrWhiteSpace(directoryPath))
                return;

            object something = _dialogService.ShowInputDialog();


            try
            {
                _binaryService.ImportDirectory(directoryPath, CurrentGame);
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private void ImportArchive()
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
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }
    }
}
