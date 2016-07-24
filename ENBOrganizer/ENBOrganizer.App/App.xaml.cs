using System;
using System.Windows;

namespace ENBOrganizer.App
{
    public partial class App : Application
    {
        public App() 
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            MessageBox.Show(((Exception)eventArgs.ExceptionObject).Message, "Unhandled Error");
        }
    }
}