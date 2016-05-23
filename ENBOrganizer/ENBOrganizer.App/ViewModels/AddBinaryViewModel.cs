using ENBOrganizer.App.Messages;
using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class AddBinaryViewModel : ViewModelBase
    {
        private readonly FileSystemService<Binary> _binaryService;
        private readonly DialogService _dialogService;

        private Game CurrentGame { get { return Settings.Default.CurrentGame; } }

        public ICommand BrowseForDirectoryCommand { get; set; }
        public ICommand BrowseForArchiveCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(nameof(Name), ref _name, value); }
        }

        private string _sourcePath;

        public string SourcePath
        {
            get { return _sourcePath; }
            set { Set(nameof(SourcePath), ref _sourcePath, value); }
        }

        public AddBinaryViewModel(FileSystemService<Binary> binaryService, DialogService dialogService)
        {
            _binaryService = binaryService;
            _dialogService = dialogService;

            BrowseForDirectoryCommand = new RelayCommand(BrowseForDirectory);
            BrowseForArchiveCommand = new RelayCommand(BrowseForArchive);
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Close);
        }

        private void BrowseForDirectory()
        {
            string directoryPath = _dialogService.PromptForFolder("Please select the binary folder...");

            if (string.IsNullOrWhiteSpace(directoryPath))
                return;

            SourcePath = directoryPath;
            Name = new DirectoryInfo(directoryPath).Name;
        }

        private void BrowseForArchive()
        {
            string archivePath = _dialogService.PromptForFile("Please select an archive file", "ZIP Files(*.zip) | *.zip");

            if (string.IsNullOrWhiteSpace(archivePath))
                return;

            SourcePath = archivePath;
            Name = Path.GetFileNameWithoutExtension(SourcePath);
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(SourcePath)
                && (Directory.Exists(SourcePath) || File.Exists(SourcePath));
        }

        private void Save()
        {
            try
            {
                _binaryService.Import(new Binary(Name, CurrentGame), SourcePath);
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
            finally
            {
                Close();
            }
        }

        private void Close()
        {
            Name = string.Empty;
            SourcePath = string.Empty;

            _dialogService.CloseDialog(DialogName.AddBinary);
        }
    }
}
