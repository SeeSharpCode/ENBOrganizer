using System.Windows.Forms;

namespace ENBOrganizer.Util.UI
{
    public static class MessageBoxUtil
    {
        public static void ShowInfo(string message)
        {
            MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult PromptForDecision(string message)
        {
            return MessageBox.Show(message, "Need Your Input", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
