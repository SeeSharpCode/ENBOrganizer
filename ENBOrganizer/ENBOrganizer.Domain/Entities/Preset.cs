using System.IO;

namespace ENBOrganizer.Domain.Entities
{
    public class Preset : FileSystemEntity
    {
        public long PresetId { get; set; }
        public Binary Binary { get; set; }

        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                RaisePropertyChanged(nameof(ImagePath));
            }
        }
        
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
