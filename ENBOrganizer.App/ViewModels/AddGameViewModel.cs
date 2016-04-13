using ENBOrganizer.Domain.Services;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class AddGameViewModel : ObservableObject
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

            BrowseCommand = new ActionCommand(BrowseForGameFile, () => true);
            AddGameCommand = new ActionCommand(AddGame, CanAdd);
        }

        private void AddGame()
        {
            try
            {
                _gameService.Add(new Game(Name, ExecutablePath));
            }
            catch (InvalidOperationException exception)
            {
                // TODO: MessageBoxUtil.ShowError(exception.Message);
            }
            finally
            {
                _gamesViewModel.IsAddGameFlyoutOpen = false;
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(ExecutablePath);
        }

        private void BrowseForGameFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "EXE Files (*.exe)|*.exe",
                Title = "Select the game's .exe file"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                ExecutablePath = openFileDialog.FileName;
            }
        }
    }
}
