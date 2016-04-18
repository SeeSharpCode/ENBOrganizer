using ENBOrganizer.Model.Entities;

namespace ENBOrganizer.App.Messages
{
    public class PresetDetailNavigationMessage : NavigationMessage
    {
        public Preset Preset { get; set; }

        public PresetDetailNavigationMessage(Preset preset)
            : base(ViewNames.PresetDetail)
        {
            Preset = preset;
        }
    }
}
