using System.Windows.Input;

namespace ENBOrganizer.App
{
    public class TitledCommand
    {
        public string Title { get; set; }
        public string Tooltip { get; set; }
        public ICommand Command { get; set; }

        public TitledCommand(string title, string tooltip, ICommand command)
        {
            Title = title;
            Tooltip = tooltip;
            Command = command;
        }
    }
}