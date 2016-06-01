using ENBOrganizer.App.Messages;
using ENBOrganizer.App.Properties;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class AddBinaryViewModel : DialogViewModelBase
    {
        private readonly FileSystemService<Binary> _binaryService;

        private Game CurrentGame { get { return Settings.Default.CurrentGame; } }

        public ICommand BrowseForDirectoryCommand { get; set; }
        public ICommand BrowseForArchiveCommand { get; set; }

        private string _sourcePath;

        public string SourcePath
        {
            get { return _sourcePath; }
            set { Set(nameof(SourcePath), ref _sourcePath, value); }
        }

        public AddBinaryViewModel(FileSystemService<Binary> binaryService)
        {
            _binaryService = binaryService;

            BrowseForDirectoryCommand = new RelayCommand(BrowseForDirectory);
            BrowseForArchiveCommand = new RelayCommand(BrowseForArchive);
        }

        private void BrowseForDirectory()
        {
            string directoryPath = _dialogService.ShowFolderBrowserDialog("Please select the binary folder...");

            if (string.IsNullOrWhiteSpace(directoryPath))
                return;

            SourcePath = directoryPath;
            Name = new DirectoryInfo(directoryPath).Name;
        }

        private void BrowseForArchive()
        {
            string archivePath = _dialogService.ShowOpenFileDialog("Please select an archive file", "ZIP Files(*.zip) | *.zip");

            if (string.IsNullOrWhiteSpace(archivePath))
                return;

            SourcePath = archivePath;
            Name = Path.GetFileNameWithoutExtension(SourcePath);
        }

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(SourcePath)
                && (Directory.Exists(SourcePath) || File.Exists(SourcePath));
        }

        protected override void Save()
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

        protected override void Close()
        {
            Name = string.Empty;
            SourcePath = string.Empty;

            _dialogService.CloseDialog(DialogName.AddBinary);
        }
    }
}