using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace ENBOrganizer.App.ViewModels.Presets
{
    public class EditPresetViewModel : DialogViewModelBase
    {
        private readonly PresetService _presetService;
        private readonly FileSystemService<Binary> _binaryService;
        private Preset _preset;

        private bool _isGlobalENBLocalEnabled;

        public bool IsGlobalENBLocalEnabled
        {
            get { return _isGlobalENBLocalEnabled; }
            set { Set(nameof(IsGlobalENBLocalEnabled), ref _isGlobalENBLocalEnabled, value); }
        }
        
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

        public ObservableCollection<Binary> Binaries { get; set; }

        public EditPresetViewModel(PresetService presetService, FileSystemService<Binary> binaryService)
        {
            _presetService = presetService;
            _binaryService = binaryService;
            _binaryService.ItemsChanged += _binaryService_ItemsChanged;
            _settingsService.PropertyChanged += _settingsService_PropertyChanged;

            MessengerInstance.Register<Preset>(this, OnPresetReceived);

            ValidatedProperties = new List<string> { nameof(Name) };

            LoadBinaries();
        }

        private void LoadBinaries()
        {
            if (Binaries == null)
                Binaries = new ObservableCollection<Binary>();

            Binaries.Clear();
            Binaries.AddAll(_binaryService.GetByGame(_settingsService.CurrentGame));
        }

        private void _settingsService_PropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.EqualsIgnoreCase("CurrentGame"))
                LoadBinaries();
        }

        private void _binaryService_ItemsChanged(object sender, RepositoryChangedEventArgs repositoryChangedEventArgs)
        {
            if (repositoryChangedEventArgs.RepositoryActionType == RepositoryActionType.Added)
                Binaries.Add(repositoryChangedEventArgs.Entity as Binary);
            else
                Binaries.Remove(repositoryChangedEventArgs.Entity as Binary);
        }

        private void OnPresetReceived(Preset preset)
        {
            _preset = preset;

            Name = _preset.Name;
            IsGlobalENBLocalEnabled = _preset.IsGlobalENBLocalEnabled;
            Description = _preset.Description;
            Binary = _preset.Binary;
        }
        
        protected override void Save()
        {
            try
            {
                if (NoChanges())
                    return;
                
                if (!_preset.Name.EqualsIgnoreCase(Name.Trim()))
                {
                    if (_presetService.GetByGame(_settingsService.CurrentGame).Any(preset => preset.Name.EqualsIgnoreCase(Name.Trim())))
                        _dialogService.ShowWarningDialog("Cannot use this name as another preset already has this name. Other changes will be saved.");
                    else
                        _presetService.Rename(_preset, Name.Trim());
                }
                    
                if (Binary == null || (Binary.Name == "-- None --" && Binary.Game == null))
                    Binary = null;

                _preset.IsGlobalENBLocalEnabled = IsGlobalENBLocalEnabled;
                _preset.Description = Description?.Trim();
                _preset.Binary = Binary;

                _presetService.SaveChanges();
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

        private bool NoChanges()
        {
            return Name.Trim().EqualsIgnoreCase(_preset.Name) 
                && CompareDescriptions() 
                && CompareBinaries() 
                && IsGlobalENBLocalEnabled == _preset.IsGlobalENBLocalEnabled;
        }

        private bool CompareDescriptions()
        {
            return Description == null ? _preset.Description == null : Description.Trim().EqualsIgnoreCase(_preset.Description);
        }

        private bool CompareBinaries()
        {
            return Binary == null ? _preset.Binary == null : Binary.Equals(_preset.Binary);
        }
        
        protected override void Close()
        {
            _preset = null;

            Name = string.Empty;
            Description = string.Empty;
            Binary = null;

            _dialogService.CloseDialog(DialogName.EditPreset);
        }

        protected override string GetValidationError(string propertyName)
        {
            return ValidateFileSystemName();
        }
    }
}
