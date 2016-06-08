using System;
using System.Windows;

namespace ENBOrganizer.App
{
    // TODO: add top margin to presets
    public partial class App : Application
    {
        public App() 
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            MessageBox.Show(((Exception)eventArgs.ExceptionObject).Message);
        }
    }
}
