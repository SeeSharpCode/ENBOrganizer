using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENBOrganizer.App.ViewModels
{
    public class PresetsOverviewViewModel : ObservableObject
    {
        private readonly PresetService _presetService;
        private readonly GameService _gameService;

        public ObservableCollection<Preset> Presets { get; set; }

        public PresetsOverviewViewModel(PresetService presetService, GameService gameService)
        {
            _presetService = presetService;
            _gameService = gameService;

            Presets = _presetService.GetByGame(_gameService.CurrentGame).ToObservableCollection();
        }
    }
}
