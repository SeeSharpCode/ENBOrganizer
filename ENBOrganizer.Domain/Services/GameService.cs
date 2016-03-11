using System;
using System.Collections.Generic;
using ENBOrganizer.Data;
using ENBOrganizer.Model;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using ENBOrganizer.Util.IO;

namespace ENBOrganizer.Domain.Services
{
    public class GameService : ObservableObject
    {
        private readonly Repository<Game> _gameRepository;

        public event EventHandler<RepositoryChangedEventArgs> GamesChanged;

        public Game ActiveGame
        {
            get { return Properties.Settings.Default.ActiveGame; }
            set
            {
                if (Properties.Settings.Default.ActiveGame == value)
                    return;

                Properties.Settings.Default.ActiveGame = value;
                Properties.Settings.Default.Save();

                RaisePropertyChanged("ActiveGame");
            }
        }

        public GameService()
            : this(new Repository<Game>(RepositoryFileNames.Games))
        { }

        public GameService(Repository<Game> gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public List<Game> GetAll()
        {
            return _gameRepository.GetAll();
        }

        public void Add(Game game)
        {
            try
            {
                game.PresetsDirectory.Create();

                _gameRepository.Add(game);

                if (ActiveGame == null)
                    ActiveGame = game;

                RaiseGamesChanged(new RepositoryChangedEventArgs(RepositoryActionType.Added, game));
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        // TODO: delete all presets tied to game (dependent on whether we need the preset store)
        public void Delete(Game game)
        {
            try
            {
                game.PresetsDirectory.DeleteRecursive();

                _gameRepository.Delete(game);

                if (ActiveGame != null && ActiveGame.Equals(game))
                    ActiveGame = null;

                RaiseGamesChanged(new RepositoryChangedEventArgs(RepositoryActionType.Deleted, game));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RaiseGamesChanged(RepositoryChangedEventArgs eventArgs)
        {
            if (GamesChanged != null)
                GamesChanged(this, eventArgs);
        }
    }
}
