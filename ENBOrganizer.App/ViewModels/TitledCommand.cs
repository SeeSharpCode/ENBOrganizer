using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels
{
    public class TitledCommand
    {
        public string Title { get; set; }
        public ICommand Command { get; set; }

        public TitledCommand(string title, ICommand command)
        {
            Title = title;
            Command = command;
        }
    }
}
