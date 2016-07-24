using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using MadMilkman.Ini;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ENBOrganizer.App.ViewModels.Presets
{
    public class AddENBoostPresetViewModel : DialogViewModelBase
    {
        private readonly FileSystemService<Binary> _binaryService;
        private readonly PresetService _presetService;

        public ObservableCollection<Binary> Binaries { get; set; }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { Set(nameof(Description), ref _description, value); }
        }

        private Binary _binary;

        public Binary Binary
        {
            get { return _binary; }
            set { Set(nameof(Binary), ref _binary, value); }
        }

        private string _reservedMemorySize;

        public string ReservedMemorySize
        {
            get { return _reservedMemorySize; }
            set { Set(nameof(ReservedMemorySize), ref _reservedMemorySize, value); }
        }

        private string _videoMemorySize;

        public string VideoMemorySize
        {
            get { return _videoMemorySize; }
            set { Set(nameof(VideoMemorySize), ref _videoMemorySize, value); }
        }

        private bool _isFPSLimiterEnabled;

        public bool IsFPSLimiterEnabled
        {
            get { return _isFPSLimiterEnabled; }
            set { Set(nameof(IsFPSLimiterEnabled), ref _isFPSLimiterEnabled, value); }
        }

        private bool _isForceBorderlessEnabled;

        public bool IsForceBorderlessEnabled
        {
            get { return _isForceBorderlessEnabled; }
            set { Set(nameof(IsForceBorderlessEnabled), ref _isForceBorderlessEnabled, value); }
        }

        private bool _isForceBorderlessFullscreenEnabled;

        public bool IsForceBorderlessFullscreenEnabled
        {
            get { return _isForceBorderlessFullscreenEnabled; }
            set { Set(nameof(IsForceBorderlessFullscreenEnabled), ref _isForceBorderlessFullscreenEnabled, value); }
        }

        private string _fpsLimit;

        public string FPSLimit
        {
            get { return _fpsLimit; }
            set { Set(nameof(FPSLimit), ref _fpsLimit, value); }
        }

        private bool _isVsyncEnabled;

        public bool IsVsyncEnabled
        {
            get { return _isVsyncEnabled; }
            set { Set(nameof(IsVsyncEnabled), ref _isVsyncEnabled, value); }
        }

        private bool _openFileAfterSaving;

        public bool OpenFileAfterSaving
        {
            get { return _openFileAfterSaving; }
            set { Set(nameof(OpenFileAfterSaving), ref _openFileAfterSaving, value); }
        }

        public AddENBoostPresetViewModel(FileSystemService<Binary> binaryService, PresetService presetService)
        {
            _presetService = presetService;
            _binaryService = binaryService;
            _binaryService.ItemsChanged += _binaryService_ItemsChanged;
            _settingsService.PropertyChanged += _settingsService_PropertyChanged;

            IsVsyncEnabled = true;

            ValidatedProperties = new List<string> { nameof(Name) };

            Binaries = new ObservableCollection<Binary>();
            LoadBinaries();
        }

        private void _settingsService_PropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.EqualsIgnoreCase("CurrentGame"))
                LoadBinaries();
        }

        private void LoadBinaries()
        {            
            Binaries.Clear();
            Binaries.AddAll(_binaryService.GetByGame(_settingsService.CurrentGame));
        }

        private void _binaryService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                Binaries.Add(repositoryChangedEventArgs.Entity as Binary);
            else
                Binaries.Remove(repositoryChangedEventArgs.Entity as Binary);
        }

        protected override void Close()
        {
            Name = string.Empty;
            Description = string.Empty;
            Binary = null;

            _dialogService.CloseDialog(DialogName.AddENBoostPreset);
        }

        protected override string GetValidationError(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    return ValidateFileSystemName();
            }

            return string.Empty;
        }

        protected override void Save()
        {
            try
            {
                Preset preset = new Preset(Name.Trim(), _settingsService.CurrentGame) { Description = Description?.Trim() };

                // Detect whether the user has selected the default value in the ComboBox.
                if (Binary != null && Binary.Name != "-- None --" && Binary.Game != null)
                    preset.Binary = Binary;

                AddPreset(preset);

                SaveENBLocalFileToPreset(preset);
            }
            catch (DuplicateEntityException)
            {
                _dialogService.ShowErrorDialog("A preset with this name already exists for the current game.");
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog("Error adding preset." + Environment.NewLine + exception.Message);
            }
            finally
            {
                Close();
            }
        }

        private void AddPreset(Preset preset)
        {     
            _presetService.Add(preset);
            preset.Directory.Create();
        }

        private void SaveENBLocalFileToPreset(Preset preset)
        {
            IniFile iniFile = new IniFile();
            iniFile.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("ENBOrganizer.App.ENBLocalFiles.v308-enboost.ini"));

            SetENBLocalValues(iniFile);

            string filePath = Path.Combine(preset.Directory.FullName, "enblocal.ini");
            iniFile.Save(filePath);

            if (!OpenFileAfterSaving)
                return;

            try
            {
                Process.Start(filePath);
            }
            catch (Exception) { }
        }

        private void SetENBLocalValues(IniFile iniFile)
        {
            iniFile.Sections["ENGINE"].Keys["EnableVSync"].Value = IsVsyncEnabled.ToString().ToLower();
            iniFile.Sections["WINDOW"].Keys["ForceBorderless"].Value = IsForceBorderlessEnabled.ToString().ToLower();
            iniFile.Sections["WINDOW"].Keys["ForceBorderlessFullscreen"].Value = IsForceBorderlessFullscreenEnabled.ToString().ToLower();
            iniFile.Sections["MEMORY"].Keys["ReservedMemorySizeMb"].Value = ReservedMemorySize;
            iniFile.Sections["MEMORY"].Keys["VideoMemorySizeMb"].Value = GetVideoMemorySize();
            iniFile.Sections["LIMITER"].Keys["EnableFPSLimit"].Value = IsFPSLimiterEnabled.ToString().ToLower();

            if (IsFPSLimiterEnabled)
                iniFile.Sections["LIMITER"].Keys["FPSLimit"].Value = GetFPSLimit();
        }

        private string GetFPSLimit()
        {
            try
            {
                return int.Parse(FPSLimit).ToString();
            }
            catch (Exception)
            {
                return "60.0";
            }
        }

        private string GetVideoMemorySize()
        {
            try
            {
                return int.Parse(VideoMemorySize).ToString();
            }
            catch (Exception)
            {
                return "2000";
            }
        }
    }
}
