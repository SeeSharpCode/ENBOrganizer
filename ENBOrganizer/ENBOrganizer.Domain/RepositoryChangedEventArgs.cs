using System;
using ENBOrganizer.Domain.Entities;

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
        public EntityBase Entity { get; set; }

        public RepositoryChangedEventArgs(RepositoryActionType gamesChangedActionType, EntityBase entity)
        {
            RepositoryActionType = gamesChangedActionType;
            Entity = entity;
        }
    }
}
