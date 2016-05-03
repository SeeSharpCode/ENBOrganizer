using GalaSoft.MvvmLight.Messaging;

namespace ENBOrganizer.App.Messages
{
    public enum ViewName
    {
        PresetsOverview,
        PresetDetail
    }

    public class NavigationMessage : MessageBase
    {
        public ViewName ViewName { get; set; }

        public NavigationMessage(ViewName viewName)
        {
            ViewName = viewName;
        }
    }
}
