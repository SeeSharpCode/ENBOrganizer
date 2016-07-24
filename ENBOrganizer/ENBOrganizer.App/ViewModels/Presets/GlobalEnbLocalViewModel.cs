using ENBOrganizer.App.Messages;
using GalaSoft.MvvmLight.CommandWpf;
using MadMilkman.Ini;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Input;

namespace ENBOrganizer.App.ViewModels.Presets
{
    public class GlobalEnbLocalViewModel : DialogViewModelBase
    {
        public ICommand GenerateENBLocalCommand { get; set; }

        private string _binaryVersion;

        public string BinaryVersion
        {
            get { return _binaryVersion; }
            set { Set(nameof(BinaryVersion), ref _binaryVersion, value); }
        }

        private string _reservedMemorySize;

        public string ReservedMemorySize
        {
            get { return _reservedMemorySize; }
            set { Set(nameof(ReservedMemorySize), ref _reservedMemorySize, value); }
        }

        private string _videoMemorySize;

        public string VideoMemorySize
        {
            get { return _videoMemorySize; }
            set { Set(nameof(VideoMemorySize), ref _videoMemorySize, value); }
        }

        private bool _isFPSLimiterEnabled;

        public bool IsFPSLimiterEnabled
        {
            get { return _isFPSLimiterEnabled; }
            set { Set(nameof(IsFPSLimiterEnabled), ref _isFPSLimiterEnabled, value); }
        }

        private bool _isForceBorderlessEnabled;

        public bool IsForceBorderlessEnabled
        {
            get { return _isForceBorderlessEnabled; }
            set { Set(nameof(IsForceBorderlessEnabled), ref _isForceBorderlessEnabled, value); }
        }

        private bool _isForceBorderlessFullscreenEnabled;

        public bool IsForceBorderlessFullscreenEnabled
        {
            get { return _isForceBorderlessFullscreenEnabled; }
            set { Set(nameof(IsForceBorderlessFullscreenEnabled), ref _isForceBorderlessFullscreenEnabled, value); }
        }

        private string _fpsLimit;

        public string FPSLimit
        {
            get { return _fpsLimit; }
            set { Set(nameof(FPSLimit), ref _fpsLimit, value); }
        }

        private bool _isVsyncEnabled;

        public bool IsVsyncEnabled
        {
            get { return _isVsyncEnabled; }
            set { Set(nameof(IsVsyncEnabled), ref _isVsyncEnabled, value); }
        }

        private string _iniFileText;

        public string INIFileText
        {
            get { return _iniFileText; }
            set { Set(nameof(INIFileText), ref _iniFileText, value); }
        }
        
        public GlobalEnbLocalViewModel()
        {            
            GenerateENBLocalCommand = new RelayCommand(GenerateENBLocal);

            if (_settingsService.CurrentGame.GlobalENBLocalFile.Exists)
                INIFileText = File.ReadAllText(_settingsService.CurrentGame.GlobalENBLocalFile.FullName);

            MessengerInstance.Register<DialogMessage>(this, OnDialogMessageReceived);
        }

        private void OnDialogMessageReceived(DialogMessage message)
        {
            if (message.DialogAction == DialogAction.Open && message.DialogName == DialogName.GlobalEnbLocal)
            {
                if (_settingsService.CurrentGame.GlobalENBLocalFile.Exists)
                    INIFileText = File.ReadAllText(_settingsService.CurrentGame.GlobalENBLocalFile.FullName);
            }
        }

        private void GenerateENBLocal()
        {
            try
            {
                string resourceName = string.Format("ENBOrganizer.App.ENBLocalFiles.{0}",
                (BinaryVersion == "v279/v292" ? "v292" : BinaryVersion) + ".ini");

                IniFile iniFile = new IniFile();
                iniFile.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));

                SetENBLocalValues(iniFile);

                iniFile.Save(_settingsService.CurrentGame.GlobalENBLocalFile.FullName);

                INIFileText = File.ReadAllText(_settingsService.CurrentGame.GlobalENBLocalFile.FullName);

                _dialogService.ShowInfoDialog("Global enblocal.ini saved successfully.\nYou can edit the file in the right pane if needed.");
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
        }

        private void SetENBLocalValues(IniFile iniFile)
        {
            iniFile.Sections["ENGINE"].Keys["EnableVSync"].Value = IsVsyncEnabled.ToString().ToLower();
            iniFile.Sections["WINDOW"].Keys["ForceBorderless"].Value = IsForceBorderlessEnabled.ToString().ToLower();
            iniFile.Sections["WINDOW"].Keys["ForceBorderlessFullscreen"].Value = IsForceBorderlessFullscreenEnabled.ToString().ToLower();
            iniFile.Sections["MEMORY"].Keys["ReservedMemorySizeMb"].Value = ReservedMemorySize;
            iniFile.Sections["MEMORY"].Keys["VideoMemorySizeMb"].Value = GetVideoMemorySize();
            iniFile.Sections["LIMITER"].Keys["EnableFPSLimit"].Value = IsFPSLimiterEnabled.ToString().ToLower();

            if (IsFPSLimiterEnabled)
                iniFile.Sections["LIMITER"].Keys["FPSLimit"].Value = GetFPSLimit();
        }

        private string GetFPSLimit()
        {
            try
            {
                return int.Parse(FPSLimit).ToString();
            }
            catch (Exception)
            {
                return "60.0";
            }
        }

        private string GetVideoMemorySize()
        {
            try
            {
                return int.Parse(VideoMemorySize).ToString();
            }
            catch (Exception)
            {
                return "2000";
            }
        }

        protected override string GetValidationError(string propertyName)
        {
            throw new NotImplementedException();
        }

        protected override void Save()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(INIFileText))
                    File.WriteAllText(_settingsService.CurrentGame.GlobalENBLocalFile.FullName, INIFileText);
            }
            catch (Exception exception)
            {
                _dialogService.ShowErrorDialog(exception.Message);
            }
            finally
            {
                Close();
            }
        }

        protected override void Close()
        {
            INIFileText = string.Empty;

            _dialogService.CloseDialog(DialogName.GlobalEnbLocal);
        }
    }
}
