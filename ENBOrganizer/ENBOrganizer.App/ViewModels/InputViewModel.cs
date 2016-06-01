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

        private string _input;

        public string Input
        {
            get { return _input; }
            set { Set(nameof(Input), ref _input, value); }
        }

        protected override void Save()
        {
            Close();
        }

        protected override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Input);
        }

        protected override void Close()
        {
            DialogHost.CloseDialogCommand.Execute(Input, null);

            Prompt = string.Empty;
            Input = string.Empty;
        }
    }
}