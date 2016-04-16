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

        public static GameService GameService { get { return _gameService; } }
        
        private readonly static PresetsViewModel _presetsViewModel = new PresetsViewModel(_presetService, GameService, _presetItemsService);
        private readonly static GamesViewModel _gamesViewModel = new GamesViewModel(GameService);
        private readonly static MainViewModel _mainViewModel = new MainViewModel(_gamesViewModel);
        private readonly static AddGameViewModel _addGameViewModel = new AddGameViewModel(GameService, _gamesViewModel);
        private readonly static PresetsOverviewViewModel _presetsOverviewViewModel = new PresetsOverviewViewModel(_presetService, GameService);
        private readonly static PresetDetailViewModel _presetDetailViewModel = new PresetDetailViewModel(_presetItemsService);

        public static PresetsViewModel PresetsViewModel { get { return _presetsViewModel; } }
        public static GamesViewModel GamesViewModel { get { return _gamesViewModel; } }
        public static MainViewModel MainViewModel { get { return _mainViewModel; } }
        public static AddGameViewModel AddGameViewModel { get { return _addGameViewModel; } }
        public static PresetsOverviewViewModel PresetsOverviewViewModel { get { return _presetsOverviewViewModel; } }
        public static PresetDetailViewModel PresetDetailViewModel { get { return _presetDetailViewModel; } }
    }
}
