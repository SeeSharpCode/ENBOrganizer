using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace ENBOrganizer.App.Messages
{
    public class NavigationMessage : MessageBase
    {
        public ViewModelBase ViewModel { get; set; }

        public NavigationMessage(ViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
