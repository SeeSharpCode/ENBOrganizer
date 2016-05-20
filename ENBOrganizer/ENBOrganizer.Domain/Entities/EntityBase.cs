using System;
using System.ComponentModel;

namespace ENBOrganizer.Domain.Entities
{
    public class EntityBase : INotifyPropertyChanged
    {
        public string ID { get; set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public EntityBase() { } // Required for serialization.

        public EntityBase(string name)
        {
            Name = name;
            ID = Guid.NewGuid().ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}