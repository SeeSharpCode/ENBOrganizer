using ENBOrganizer.App.Messages;
using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Domain.Services;
using ENBOrganizer.Util;
using ENBOrganizer.Util.IO;
using GalaSoft.MvvmLight.CommandWpf;
using MvvmValidation;
using System.IO;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels.Games
{
    public class GameDetailViewModel : DialogViewModelBase
    {
        private readonly GameService _gameService;
        private Game _game;

        public ICommand BrowseCommand { get; set; }

        private string _executablePath;
        
        public string ExecutablePath
        {
            get { return _executablePath; }
            set
            {
                _executablePath = value;
                _validator.Validate(() => ExecutablePath);
                RaisePropertyChanged(nameof(ExecutablePath));
            }
        }

        public GameDetailViewModel(GameService gameService)
        {
            _gameService = gameService;

            MessengerInstance.Register<Game>(this, OnGameReceived);

            BrowseCommand = new RelayCommand(BrowseForGameFile);
        }

        private void OnGameReceived(Game game)
        {
            _game = game;

            Name = game.Name;
            ExecutablePath = game.ExecutablePath;
        }

        protected override void Close()
        {
            _game = null;

            Name = string.Empty;
            ExecutablePath = string.Empty;

            _dialogService.CloseDialog(DialogName.GameDetail);
        }

        protected override void Save()
        {
            try
            {
                if (_game == null)
                    _gameService.Add(new Game(Name.Trim(), ExecutablePath.Trim()));
                else
                {
                    bool updateCurrentGame = _settingsService.CurrentGame.Equals(_game);
                    
                    if (!_game.Name.EqualsIgnoreCase(Name.Trim()))
                        _gameService.Rename(_game, new Game(Name.Trim(), ExecutablePath.Trim()));
                    
                    _game.Name = Name.Trim();
                    _game.ExecutablePath = ExecutablePath.Trim();

                    _gameService.SaveChanges();

                    if (updateCurrentGame)
                        _settingsService.CurrentGame = _game;
                }
                    
            }
            catch (DuplicateEntityException)
            {
                _dialogService.ShowErrorDialog("Can't save this game because a game already exists with this name or directory.");
            }
            finally
            {
                Close();
            }
        }

        private void BrowseForGameFile()
        {
            string gameFilePath = _dialogService.ShowOpenFileDialog("Select the game's .exe file", "EXE Files (*.exe)|*.exe");

            if (string.IsNullOrWhiteSpace(gameFilePath))
                return;

            Name = Path.GetFileNameWithoutExtension(gameFilePath);
            ExecutablePath = gameFilePath;
        }

        protected override void SetupValidationRules()
        {
            base.SetupValidationRules();

            _validator.AddRule(() => ExecutablePath, () => RuleResult.Assert(File.Exists(ExecutablePath), "File does not exist."));
        }
    }
}
