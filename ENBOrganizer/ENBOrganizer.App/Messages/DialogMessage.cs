using GalaSoft.MvvmLight.Messaging;

namespace ENBOrganizer.App.Messages
{
    public enum DialogAction
    {
        Open,
        Close
    }

    public class DialogMessage : MessageBase
    {
        public DialogAction DialogAction { get; set; }

        public DialogMessage(DialogAction dialogAction)
        {
            DialogAction = dialogAction;
        }
    }
}
