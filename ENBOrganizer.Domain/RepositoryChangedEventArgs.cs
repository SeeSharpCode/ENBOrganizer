using System;
using ENBOrganizer.Model.Entities;

namespace ENBOrganizer.Domain
{
    public enum RepositoryActionType
    {
        Added,
        Deleted
    }

    public class RepositoryChangedEventArgs : EventArgs
    {
        public RepositoryActionType RepositoryActionType { get; set; }
        public IEntity Entity { get; set; }

        public RepositoryChangedEventArgs(RepositoryActionType gamesChangedActionType, IEntity entity)
        {
            RepositoryActionType = gamesChangedActionType;
            Entity = entity;
        }
    }
}
