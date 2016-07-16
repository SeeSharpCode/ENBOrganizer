using GalaSoft.MvvmLight.Ioc;
using System.Windows;

namespace ENBOrganizer.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private readonly SettingsService _settingsService;

        public MainView()
        {
            InitializeComponent();

            _settingsService = SimpleIoc.Default.GetInstance<SettingsService>();

            _settingsService.LoadWindowPosition(this);
            SizeWindowToFit();
            MoveWindowIntoView();
        }

        private void SizeWindowToFit()
        {
            Height = Height <= SystemParameters.VirtualScreenHeight ? Height : SystemParameters.VirtualScreenHeight;
            Width = Width <= SystemParameters.VirtualScreenWidth ? Width : SystemParameters.VirtualScreenWidth;
        }

        public void MoveWindowIntoView()
        {
            if (Top + Height / 2 > SystemParameters.VirtualScreenHeight)
                Top = SystemParameters.VirtualScreenHeight - Height;

            if (Left + Width / 2 > SystemParameters.VirtualScreenWidth)
                Left = SystemParameters.VirtualScreenWidth - Width;

            Top = Top >= 0 ? Top : 0;
            Left = Left >= 0 ? Left : 0;
        }

        private void RootElement_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _settingsService.SaveWindowPosition(this);
        }
    }
}