using ENBOrganizer.Data;
using ENBOrganizer.Model.Entities;
using System;
using System.Collections.Generic;

namespace ENBOrganizer.Domain.Services
{
    public class DataService<TEntity> where TEntity : IEntity
    {
        protected readonly Repository<TEntity> _repository;

        public event EventHandler<RepositoryChangedEventArgs> ItemsChanged;

        public DataService()
        {
            _repository = new Repository<TEntity>(typeof(TEntity).Name + "s.xml");
        }

        public List<TEntity> GetAll()
        {
            return _repository.Items;
        }

        public void Add(TEntity entity)
        {
            _repository.Add(entity);

            RaiseItemsChanged(new RepositoryChangedEventArgs(RepositoryActionType.Added, entity));
        }

        public void Delete(TEntity entity)
        {
            _repository.Delete(entity);

            RaiseItemsChanged(new RepositoryChangedEventArgs(RepositoryActionType.Deleted, entity));
        }

        public void RaiseItemsChanged(RepositoryChangedEventArgs eventArgs)
        {
            ItemsChanged?.Invoke(this, eventArgs);
        }
    }
}
