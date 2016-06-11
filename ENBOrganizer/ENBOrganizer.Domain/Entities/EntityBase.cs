using System.ComponentModel;

namespace ENBOrganizer.Domain.Entities
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public abstract class EntityBase : INotifyPropertyChanged
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        public EntityBase() { }
        
        public EntityBase(string name)
        {
            Name = name;
        }
        
        public abstract override bool Equals(object other);

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}