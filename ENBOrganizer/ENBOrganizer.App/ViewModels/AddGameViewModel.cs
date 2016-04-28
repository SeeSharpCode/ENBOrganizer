using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Domain.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class AddGameViewModel : ViewModelBase
    {
        private readonly GameService _gameService;
        private readonly GamesViewModel _gamesViewModel;

        public ICommand BrowseCommand { get; set; }
        public ICommand AddGameCommand { get; set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value.Trim();
                RaisePropertyChanged("Name");
            }
        }

        private string _executablePath;

        public string ExecutablePath
        {
            get { return _executablePath; }
            set
            {
                _executablePath = value;
                RaisePropertyChanged("ExecutablePath");
            }
        }

        public AddGameViewModel(GameService gameService, GamesViewModel gamesViewModel)
        {
            _gameService = gameService;
            _gamesViewModel = gamesViewModel;

            BrowseCommand = new RelayCommand(BrowseForGameFile);
            AddGameCommand = new RelayCommand(AddGame, CanAdd);
        }

        private async void AddGame()
        {
            try
            {
                _gameService.Add(new Game(Name, ExecutablePath));
            }
            catch (DuplicateEntityException exception)
            {
                await DialogService.ShowErrorDialog(exception.Message);
            }
            finally
            {
                Name = string.Empty;
                ExecutablePath = string.Empty;

                DialogService.CloseAddGameDialog();
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(ExecutablePath);
        }

        private void BrowseForGameFile()
        {
            string gameFilePath = DialogService.PromptForFile("Select the game's .exe file", "EXE Files (*.exe)|*.exe");

            if (gameFilePath.Equals(string.Empty))
                return;

            Name = Path.GetFileNameWithoutExtension(gameFilePath);
            ExecutablePath = gameFilePath;
        }
    }
}
