using ENBOrganizer.Util;
using System.ComponentModel;

namespace ENBOrganizer.Domain.Entities
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class EntityBase : INotifyPropertyChanged
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
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
        }

        public override bool Equals(object other)
        {
            EntityBase entity = other as EntityBase;

            return entity != null ? ID.EqualsIgnoreCase(entity.ID) : false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}