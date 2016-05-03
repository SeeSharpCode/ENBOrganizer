namespace ENBOrganizer.App.Messages
{
    public enum Dialog
    {
        AddGame,
        AddMasterListItem
    }

    public class OpenDialogMessage : DialogMessage
    {
        public Dialog Dialog { get; set; }

        public OpenDialogMessage(Dialog dialog)
            : base(DialogAction.Open)
        {
            Dialog = dialog;
        }
    }
}
