using System;
using System.Windows;

namespace ENBOrganizer.App
{
    // TODO: indeterminate progress bar on footer
    // TODO: remove unused icons
    // TODO: double-check reponsibilties of services
    // TODO: optimize UI speed
    // TODO: settings
    // TODO: add support for binaries
    // TODO: Remove exception handling when it's just catching Exception and displaying a message. This could be done globally.

    // TODO: better exception dialog in here
    public partial class App : Application
    {
        public App() 
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs eventArgs)
        {
           MessageBox.Show(eventArgs.ExceptionObject.ToString());
        }
    }
}
