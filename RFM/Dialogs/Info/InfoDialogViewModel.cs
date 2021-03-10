using System.Timers;
using Prism.Commands;
using RFM.Dialogs.Base;
using RFM.Dialogs.Common;

namespace RFM.Dialogs
{
    public class InfoDialogViewModel : DialogViewModelBase<bool?>
    {
        private AlertType _alertType;
        public AlertType AlertType
        {
            get => _alertType;
            set => SetProperty(ref _alertType, value);
        }

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

        private string _okText;
        public string OkText
        {
            get => _okText;
            set => SetProperty(ref _okText, value);
        }

        public DelegateCommand OkCommand { get; set; }

        public InfoDialogViewModel(string title, string message, AlertType alertType = AlertType.Info, string okText = "Ok")
        {
            Title = title;
            Message = message;
            AlertType = alertType;
            OkText = okText;
            OkCommand = new DelegateCommand(DoOk);
        }

        private void DoOk()
        {
            CloseDialog(true);
        }

        public override void StartTimer()
        {
            if (AutoHideIntervalSecs > 1)
            {
                _timer = new Timer(1000);
                _timer.Elapsed -= Timer_Elapsed;
                _timer.Elapsed += Timer_Elapsed;
                _timer.Start();
            }
        }

        private int timerCount = 0;
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (AutoHideIntervalSecs != null)
            {
                int autoHideSecs = AutoHideIntervalSecs.Value;
                int pendingInterval = autoHideSecs - timerCount;
                if (pendingInterval <= 0)
                {
                    CloseDialog(false);
                }
            }
            timerCount++;
        }

    }
}
