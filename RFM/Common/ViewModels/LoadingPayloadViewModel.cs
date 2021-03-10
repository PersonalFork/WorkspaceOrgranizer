using Prism.Mvvm;

namespace RFM.Common.ViewModels
{
    public class LoadingPayloadViewModel : BindableBase
    {
        #region Public Properties.

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        #endregion
    }
}
