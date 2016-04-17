using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util.IO;
using System;

namespace ENBOrganizer.Domain.Services
{
    public class GameService : DataService<Game>
    {
        public new void Add(Game game)
        {
            try
            {
                game.PresetsDirectory.Create();

                base.Add(game);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        // TODO: set CurrentGame to another game?
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
