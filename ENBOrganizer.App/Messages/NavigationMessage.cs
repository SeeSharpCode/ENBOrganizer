namespace ENBOrganizer.App.Messages
{
    public enum ViewNames
    {
        PresetsOverview,
        PresetDetail,
        AddGame
    }

    public class NavigationMessage
    {
        public ViewNames ViewName { get; set; }

        public NavigationMessage(ViewNames viewName)
        {
            ViewName = viewName;
        }
    }
}
