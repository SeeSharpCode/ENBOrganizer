using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels.Presets
{
    public class AddPresetViewModel : DialogViewModelBase
    {
        private readonly PresetService _presetService;
        private readonly FileSystemService<Binary> _binaryService;

        private string _sourcePath;

        public string SourcePath
        {
            get { return _sourcePath; }
            set { Set(nameof(SourcePath), ref _sourcePath, value.Trim()); }
        }

        public string Description { get; set; }
        public Binary Binary { get; set; }
        public ObservableCollection<Binary> Binaries { get; set; }
        public ICommand BrowseForDirectoryCommand { get; set; }
        public ICommand BrowseForArchiveCommand { get; set; }

        public AddPresetViewModel(PresetService presetService, FileSystemService<Binary> binaryService)
        {
            _presetService = presetService;
            _binaryService = binaryService;
            _binaryService.ItemsChanged += _binaryService_ItemsChanged;

            BrowseForDirectoryCommand = new RelayCommand(BrowseForDirectory);
            BrowseForArchiveCommand = new RelayCommand(BrowseForArchive);

            Binaries = _binaryService.GetByGame(CurrentGame).ToObservableCollection();
        }

        //TODO: turn into validation rules
        //protected override bool CanSave()
        //{
        //    return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(SourcePath)
        //        && (Directory.Exists(SourcePath) || File.Exists(SourcePath));
        //}

        private void _binaryService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                Binaries.Add(repositoryChangedEventArgs.Entity as Binary);
            else
                Binaries.Remove(repositoryChangedEventArgs.Entity as Binary);
        }

        protected override void Save()
        {
            try
            {
                Preset preset = new Preset(Name, CurrentGame) { Description = Description };

                // Detect whether the user has selected the default value in the ComboBox.
                if (Binary.Name != "-- None --" && Binary.Game != null)
                    preset.Binary = Binary;

                _presetService.Import(preset, SourcePath);
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

            _dialogService.CloseDialog(DialogName.AddPreset);
        }

        private void BrowseForDirectory()
        {
            string directoryPath = _dialogService.ShowFolderBrowserDialog("Please select the preset folder...");

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

        protected override void SetupValidationRules()
        {
            throw new NotImplementedException();
        }
    }
}
