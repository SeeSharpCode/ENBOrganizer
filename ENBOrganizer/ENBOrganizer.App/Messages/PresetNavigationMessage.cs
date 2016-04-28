using ENBOrganizer.Domain.Entities;

namespace ENBOrganizer.App.Messages
{
    public class PresetNavigationMessage : NavigationMessage
    {
        public Preset Preset { get; set; }

        public PresetNavigationMessage(Preset preset) 
            : base(ViewNames.PresetDetail)
        {
            Preset = preset;
        }
    }
}
