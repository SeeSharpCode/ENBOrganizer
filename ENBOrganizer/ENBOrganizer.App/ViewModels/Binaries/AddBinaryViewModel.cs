using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight.CommandWpf;
using MvvmValidation;
using System;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels.Binaries
{
    public class AddBinaryViewModel : DialogViewModelBase
    {
        private readonly FileSystemService<Binary> _binaryService;
        
        public ICommand BrowseForDirectoryCommand { get; set; }
        public ICommand BrowseForArchiveCommand { get; set; }

        private string _sourcePath;

        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                _sourcePath = value;
                _validator.Validate(() => SourcePath);
                RaisePropertyChanged(nameof(SourcePath));
            }
        }

        public AddBinaryViewModel(FileSystemService<Binary> binaryService)
        {
            _binaryService = binaryService;

            BrowseForDirectoryCommand = new RelayCommand(BrowseForDirectory);
            BrowseForArchiveCommand = new RelayCommand(BrowseForArchive);
        }

        private void BrowseForDirectory()
        {
            string path = _dialogService.ShowFolderBrowserDialog("Please select the binary folder...");

            if (string.IsNullOrWhiteSpace(path))
                return;

            SourcePath = path;
            Name = new DirectoryInfo(path).Name;
        }

        private void BrowseForArchive()
        {
            string archivePath = _dialogService.ShowOpenFileDialog("Please select an archive file", "ZIP Files(*.zip) | *.zip");

            if (string.IsNullOrWhiteSpace(archivePath))
                return;

            SourcePath = archivePath;
            Name = Path.GetFileNameWithoutExtension(SourcePath);
        }

        protected override void Save()
        {
            try
            {
                _binaryService.Import(new Binary(Name, CurrentGame), SourcePath);
            }
            catch (DuplicateEntityException)
            {
                _dialogService.ShowErrorDialog("A binary with this name already exists for the current game.");
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

        protected override void SetupValidationRules()
        {
            base.SetupValidationRules();

            _validator.AddRule(() => SourcePath, () => RuleResult.Assert(Directory.Exists(SourcePath) || File.Exists(SourcePath), "Directory/archive does not exist."));
        }
    }
}