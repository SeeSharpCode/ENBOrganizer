using System.IO;

namespace ENBOrganizer.Domain.Entities
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Preset : FileSystemEntity
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public Binary Binary { get; set; }
        public string ImagePath { get; set; }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }
        
        public override DirectoryInfo Directory
        {
            get
            {
                string path = Path.Combine(Game.PresetsDirectory.FullName, Name);
                return new DirectoryInfo(path); 
             }
        }

        public Preset() { } // Required for XML serialization.

        public Preset(string name, Game game) : base(name, game) { }

        public override void Enable()
        {
            base.Enable();

            if (Binary != null)
                Binary.Enable();
        }

        public override void Disable()
        {
            base.Disable();

            if (Binary != null)
                Binary.Disable();
        }
    }
}
