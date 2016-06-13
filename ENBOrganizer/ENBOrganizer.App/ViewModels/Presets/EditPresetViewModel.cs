using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ENBOrganizer.App.ViewModels.Presets
{
    public class EditPresetViewModel : DialogViewModelBase
    {
        private readonly PresetService _presetService;
        private readonly FileSystemService<Binary> _binaryService;
        private Preset _preset;

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

            MessengerInstance.Register<Preset>(this, OnPresetReceived);

            Binaries = _binaryService.GetByGame(_settingsService.CurrentGame).ToObservableCollection();

            ValidatedProperties = new List<string> { nameof(Name) };
        }

        private void OnPresetReceived(Preset preset)
        {
            _preset = preset;

            Name = _preset.Name;
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
                && CompareBinaries();
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
