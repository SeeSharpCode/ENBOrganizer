using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util.IO;
using System;

namespace ENBOrganizer.Domain.Services
{
    public class GameService : DataService<Game>
    {
        public Game CurrentGame
        {
            get { return Properties.Settings.Default.ActiveGame; }
            set
            {
                if (Properties.Settings.Default.ActiveGame == value)
                    return;

                Properties.Settings.Default.ActiveGame = value;
                Properties.Settings.Default.Save();

                RaisePropertyChanged("CurrentGame");
            }
        }

        public new void Add(Game game)
        {
            try
            {
                game.PresetsDirectory.Create();

                base.Add(game);

                if (CurrentGame == null)
                    CurrentGame = game;
            }
            catch (InvalidOperationException)
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

                if (CurrentGame != null && CurrentGame.Equals(game))
                    CurrentGame = null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
