using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using ENBOrganizer.Util.IO;
using MvvmValidation;
using System;
using System.Collections.ObjectModel;

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
                if (Name != _preset.Name)
                    _presetService.Rename(_preset, Name);

                _preset.Description = Description;

                if (Binary.Name == "-- None --" && Binary.Game != null)
                    Binary = null;

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
        
        protected override void Close()
        {
            _preset = null;

            Name = string.Empty;
            Description = string.Empty;
            Binary = null;

            _dialogService.CloseDialog(DialogName.EditPreset);
        }
    }
}
