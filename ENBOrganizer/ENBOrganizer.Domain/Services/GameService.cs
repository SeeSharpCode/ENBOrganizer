using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using System;

namespace ENBOrganizer.Domain.Services
{
    public class GameService : DataService<Game>
    {
        private readonly PresetService _presetService;

        public GameService(PresetService presetService)
        {
            _presetService = presetService;
        }
        
        public new void Add(Game game)
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

        public new void Delete(Game game)
        {
            _presetService.DeleteByGame(game);

            game.PresetsDirectory.Delete(true);

            base.Delete(game);
        }
    }
}