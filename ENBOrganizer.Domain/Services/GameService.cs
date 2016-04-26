using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using ENBOrganizer.Util.IO;
using System;

namespace ENBOrganizer.Domain.Services
{
    public class GameService : DataService<Game>
    {
        /// <exception cref="DuplicateEntityException" />
        public new void Add(Game game)
        {
            try
            {
                game.PresetsDirectory.Create();

                base.Add(game);
            }
            catch (DuplicateEntityException)
            {
                throw;
            }
        }
        
        public new void Delete(Game game)
        {
            try
            {
                game.PresetsDirectory.DeleteRecursive();

                base.Delete(game);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
