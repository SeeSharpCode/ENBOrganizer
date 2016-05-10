using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
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

        /// <exception cref="DuplicateEntityException" />
        /// <exception cref="IOException" />
        public new void Add(Game game)
        {
            try
            {
                base.Add(game);

                game.PresetsDirectory.Create();
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
            catch (IOException)
            {
                base.Delete(game);

                throw;
            }
        }

        public new void Delete(Game game)
        {
            base.Delete(game);

            _presetService.DeleteByGame(game);

            game.PresetsDirectory.Delete(true);
        }
    }
}