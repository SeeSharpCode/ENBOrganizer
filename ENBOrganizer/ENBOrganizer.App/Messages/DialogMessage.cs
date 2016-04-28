using GalaSoft.MvvmLight.Messaging;

namespace ENBOrganizer.App.Messages
{
    public enum DialogActions
    {
        Open,
        Close
    }

    public class DialogMessage : MessageBase
    {
        public DialogActions DialogAction { get; set; }

        public DialogMessage(DialogActions dialogAction)
        {
            DialogAction = dialogAction;
        }
    }
}
