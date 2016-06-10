using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace ENBOrganizer.Domain.Services
{
    public class GameService : DataService<Game>
    {
        private readonly PresetService _presetService;

        public GameService(PresetService presetService)
        {
            _presetService = presetService;
        }

        public override List<Game> GetAll()
        {
            List<Game> games = base.GetAll();

            foreach (Game game in games)
                game.Presets = _presetService.GetByGame(game);

            return games;
        }
        
        public override void Add(Game game)
        {
            try
            {
                base.Add(game);

                game.PresetsDirectory.Create();
                game.BinariesDirectory.Create();
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
            catch (Exception)
            {
                base.Delete(game);

                throw;
            }
        }

        public void AddGamesFromRegistry()
        {
            foreach (KeyValuePair<string, string> gameEntry in GameNames.KnownGamesDictionary)
            {
                string installPath;
                if (RegistryUtil.TryGetInstallPath(gameEntry.Key, out installPath))
                {
                    string gameName = GameNames.GameFriendlyNameMap[gameEntry.Key];
                    string path = Path.Combine(installPath, gameEntry.Value);

                    if (File.Exists(path))
                        Add(new Game(gameName, path));
                }
            }
        }

        public override void Delete(Game game)
        {
            _presetService.DeleteByGame(game);

            game.PresetsDirectory.Delete(true);

            base.Delete(game);
        }
    }
}