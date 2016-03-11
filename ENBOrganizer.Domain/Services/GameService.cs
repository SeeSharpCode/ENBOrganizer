using System;
using System.Collections.Generic;
using ENBOrganizer.Data;
using ENBOrganizer.Model;
using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util;
using ENBOrganizer.Util.IO;
using System.Linq;

namespace ENBOrganizer.Domain.Services
{
    public class GameService : ObservableObject
    {
        private readonly Repository<Game> _gameRepository;
        private readonly PresetService _presetService;

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
        {
            _gameRepository = new Repository<Game>(RepositoryFileNames.Games);
            _presetService = ServiceSingletons.PresetService;
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
        
        public void Delete(Game game)
        {
            try
            {
                game.PresetsDirectory.DeleteRecursive();

                List<Preset> presets = _presetService.GetAll().Where(preset => preset.Game.Equals(game)).ToList();

                foreach (Preset preset in presets)
                    _presetService.Delete(preset);

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
