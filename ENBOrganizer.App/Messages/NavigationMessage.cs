using GalaSoft.MvvmLight.Messaging;

namespace ENBOrganizer.App.Messages
{
    public enum ViewNames
    {
        PresetsOverview,
        PresetDetail
    }

    public class NavigationMessage : MessageBase
    {
        public ViewNames ViewName { get; set; }

        public NavigationMessage(ViewNames viewName)
        {
            ViewName = viewName;
        }
    }
}
