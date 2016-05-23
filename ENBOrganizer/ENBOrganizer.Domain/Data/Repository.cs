using ENBOrganizer.Domain.Entities;
using ENBOrganizer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ENBOrganizer.Domain.Data
{
    public class Repository<TEntity> where TEntity : EntityBase
    {
        private readonly string _fileName;

        private readonly Lazy<List<TEntity>> _items;

        public List<TEntity> Items { get { return _items.Value; } }

        public Repository(string fileName)
        {
            _fileName = fileName;
            _items = new Lazy<List<TEntity>>(Load);
        }

        private List<TEntity> Load()
        {
            // If the XML file does not exist, a new list is created and persisted to the file instead, e.g. on first launch.
            if (!File.Exists(_fileName))
                return new List<TEntity>();
            
            using (StreamReader reader = File.OpenText(_fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TEntity>));
                return (List<TEntity>)serializer.Deserialize(reader);
            }
        }

        /// <exception cref="DuplicateEntityException" />
        public void Add(TEntity entity)
        {
            if (_items.Value.Contains(entity))
                throw new DuplicateEntityException(string.Format("Unable to add duplicate {0}: {1}", entity.GetType().Name.ToLower(), entity.Name));

            _items.Value.Add(entity);

            SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _items.Value.Remove(entity);

            SaveChanges();
        }

        public void SaveChanges()
        {
            using (StreamWriter reader = File.CreateText(_fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TEntity>));
                serializer.Serialize(reader, _items.Value);
            }  
        }
    }
}