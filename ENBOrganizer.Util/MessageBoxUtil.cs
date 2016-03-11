using System.Windows;

namespace ENBOrganizer.Util
{
    public static class MessageBoxUtil
    {
        public static void ShowError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
