using GalaSoft.MvvmLight;

namespace ENBOrganizer.App.ViewModels
{
    public class InputViewModel : ViewModelBase
    {
        private string _prompt;

        public string Prompt
        {
            get { return _prompt; }
            set { Set(nameof(Prompt), ref _prompt, value); }
        }

        
    }
}
