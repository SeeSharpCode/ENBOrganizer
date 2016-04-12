using ENBOrganizer.Domain.Services;

namespace ENBOrganizer.App.ViewModels
{
    public class AddGameViewModel
    {
        private readonly GameService _gameService;

        public AddGameViewModel(GameService gameService)
        {
            _gameService = gameService;
        }
    }
}
