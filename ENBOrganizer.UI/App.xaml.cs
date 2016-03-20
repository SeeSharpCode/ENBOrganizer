using ENBOrganizer.UI.Views;
using ENBOrganizer.Util.IO;
using ENBOrganizer.Util.UI;
using System;
using System.IO;
using System.Windows;

namespace ENBOrganizer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // TODO: (UI) shared DataTemplate between ActiveGameView and ManageGamesView
        // TODO: (UI) MainView -> PresetsView -> NameTextBox validation does not update when game is selected.
        // TODO: (UI) validation on preset name TextBox
        // TODO: (UI) context menu when no item selected
        // TODO: allow the user to select where presets are stored
        // TODO: better exception handling
        // TODO: Nexus integration (let's do it!)
        // TODO: (UI) resize AddMasterListItemView
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!CanWriteToAppDirectory())
            {
                string errorMessage = "You lack write permissions to the application folder: {0}" + Environment.NewLine + Environment.NewLine
                    + "ENB Organizer cannot function without these permissions. Please move the application to a folder you have access to.";

                MessageBoxUtil.ShowError(string.Format(errorMessage, AppDomain.CurrentDomain.BaseDirectory));

                Current.Shutdown();
            }

            MainView mainView = new MainView();
            MainWindow = mainView;
            mainView.Show();
        }

        private bool CanWriteToAppDirectory()
        {
            DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            return directory.CanWrite();
        }
    }
}
