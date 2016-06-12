using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util;
using ENBOrganizer.Util.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace ENBOrganizer.Domain.Services
{
    public class GameService : DataService<Game>
    {
        private readonly PresetService _presetService;
        private readonly FileSystemService<Binary> _binaryService;

        public GameService(PresetService presetService, FileSystemService<Binary> binaryService)
        {
            _presetService = presetService;
            _binaryService = binaryService;
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

        public void Rename(Game unmodifiedGame, Game modifiedGame)
        {
            foreach (Preset preset in _presetService.GetByGame(unmodifiedGame))
                preset.Game = modifiedGame;

            foreach (Binary binary in _binaryService.GetByGame(unmodifiedGame))
                binary.Game = modifiedGame;

            _presetService.SaveChanges();
            _binaryService.SaveChanges();

            unmodifiedGame.Directory.Rename(modifiedGame.Name);
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
            _binaryService.DeleteByGame(game);

            game.Directory.DeleteRecursive();

            base.Delete(game);
        }
    }
}