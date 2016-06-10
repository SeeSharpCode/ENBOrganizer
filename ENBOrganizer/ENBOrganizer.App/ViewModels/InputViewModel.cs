using MaterialDesignThemes.Wpf;

namespace ENBOrganizer.App.ViewModels
{
    public class InputViewModel : DialogViewModelBase
    {
        private string _prompt;

        public string Prompt
        {
            get { return _prompt; }
            set { Set(nameof(Prompt), ref _prompt, value); }
        }
        
        protected override void Save()
        {
            Close();
        }

        protected override void Close()
        {
            DialogHost.CloseDialogCommand.Execute(Name, null);

            Prompt = string.Empty;
            Name = string.Empty;
        }
    }
}