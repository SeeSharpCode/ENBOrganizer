using System;

namespace ENBOrganizer.App.ViewModels.Presets
{
    public class GlobalEnbLocalViewModel : DialogViewModelBase
    {
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

        private bool _isWindowedModeEnabled;

        public bool IsWindowedModeEnabled
        {
            get { return _isWindowedModeEnabled; }
            set { Set(nameof(IsWindowedModeEnabled), ref _isWindowedModeEnabled, value); }
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
        
        public GlobalEnbLocalViewModel()
        {
        }

        protected override string GetValidationError(string propertyName)
        {
            throw new NotImplementedException();
        }

        protected override void Save()
        {
            throw new NotImplementedException();
        }

        protected override void Close()
        {
            throw new NotImplementedException();
        }
    }
}
