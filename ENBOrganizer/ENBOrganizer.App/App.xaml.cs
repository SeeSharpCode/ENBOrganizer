using System;
using System.Windows;

namespace ENBOrganizer.App
{
    // TODO: Remove exception handling when it's just catching Exception and displaying a message. This could be done globally.
    // TODO: better exception dialog in here
    // TODO: AddBinaryView and GameDetailView should use StackPanel for simplicity
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
