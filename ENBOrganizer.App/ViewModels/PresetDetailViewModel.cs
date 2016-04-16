using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using System.Collections.Generic;
using System.IO;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetDetailViewModel : ObservableObject
    {
        private readonly PresetItemsService _presetItemsService;

        private Preset _preset;

        public Preset Preset
        {
            get { return _preset; }
            set
            {
                _preset = value;
                RaisePropertyChanged("Preset");
                RaisePropertyChanged("Items");
            }
        }

        public List<IPresetItem> Items
        {
            get { return _presetItemsService.GetPresetItems(Path.Combine(Preset.Game.PresetsDirectory.FullName, Preset.Name)); }
        }

        public PresetDetailViewModel(PresetItemsService presetItemsService)
        {
            _presetItemsService = presetItemsService;
        }
    }
}
