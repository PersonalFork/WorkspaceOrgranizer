using Prism.Commands;
using RFM.Dialogs.Base;

namespace RFM.Dialogs
{
    public class ConfirmDialogViewModel : DialogViewModelBase<bool?>
    {
        #region Public Properties.

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _yesText;
        public string YesText
        {
            get => _yesText;
            set => SetProperty(ref _yesText, value);
        }

        private string _noText;
        public string NoText
        {
            get => _noText;
            set => SetProperty(ref _noText, value);
        }

        private string _cancelText;
        public string CancelText
        {
            get => _cancelText;
            set => SetProperty(ref _cancelText, value);
        }

        private string _question;
        public string Question
        {
            get => _question;
            set => SetProperty(ref _question, value);
        }

        public DelegateCommand YesCommand { get; private set; }
        public DelegateCommand NoCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        #endregion

        #region Constructors.

        public ConfirmDialogViewModel(string title, string question, string yesText = "Yes", string noText = "No", string cancelText = null)
        {
            Title = title;
            Question = question;
            YesText = yesText;
            NoText = noText;
            CancelText = cancelText;

            YesCommand = new DelegateCommand(DoAccept);
            NoCommand = new DelegateCommand(DoReject);
            CancelCommand = new DelegateCommand(DoCancel);
        }

        #endregion

        #region Private Method Declarations.

        private void DoCancel()
        {
            CloseDialog(null);
        }

        private void DoReject()
        {
            CloseDialog(false);
        }

        private void DoAccept()
        {
            CloseDialog(true);
        }

        #endregion
    }
}
