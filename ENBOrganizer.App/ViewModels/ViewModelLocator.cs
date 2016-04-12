using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;

namespace ENBOrganizer.App.ViewModels
{
    public class ViewModelLocator
    {
        private readonly static DataService<MasterListItem> _masterListItemService = new DataService<MasterListItem>();
        private readonly static PresetService _presetService = new PresetService(_masterListItemService);
        private readonly static GameService _gameService = new GameService();
        private readonly static PresetItemsService _presetItemsService = new PresetItemsService(_masterListItemService);
        
        private readonly static PresetsViewModel _presetsViewModel = new PresetsViewModel(_presetService, _gameService, _presetItemsService);
        private readonly static GamesViewModel _gamesViewModel = new GamesViewModel(_gameService);
        private readonly static MainViewModel _mainViewModel = new MainViewModel(_gamesViewModel);

        public static PresetsViewModel PresetsViewModel { get { return _presetsViewModel; } }
        public static GamesViewModel GamesViewModel { get { return _gamesViewModel; } }
        public static MainViewModel MainViewModel { get { return _mainViewModel; } }
    }
}
