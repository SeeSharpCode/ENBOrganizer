using ENBOrganizer.Model.Entities;

namespace ENBOrganizer.App
{
    public class GoToPresetDetailMessage
    {
        public Preset Preset { get; set; }

        public GoToPresetDetailMessage(Preset preset)
        {
            Preset = preset;
        }
    }
}
