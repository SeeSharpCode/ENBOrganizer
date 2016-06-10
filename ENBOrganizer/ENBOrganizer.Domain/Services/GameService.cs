using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using System;
using System.Collections.Generic;

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

        public override void Delete(Game game)
        {
            _presetService.DeleteByGame(game);

            game.PresetsDirectory.Delete(true);

            base.Delete(game);
        }
    }
}