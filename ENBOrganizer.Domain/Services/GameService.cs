using ENBOrganizer.Model.Entities;
using ENBOrganizer.Util.IO;
using System;

namespace ENBOrganizer.Domain.Services
{
    public class GameService : DataService<Game>
    {
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

        public new void Add(Game game)
        {
            try
            {
                game.PresetsDirectory.Create();

                base.Add(game);

                if (ActiveGame == null)
                    ActiveGame = game;
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

                if (ActiveGame != null && ActiveGame.Equals(game))
                    ActiveGame = null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
