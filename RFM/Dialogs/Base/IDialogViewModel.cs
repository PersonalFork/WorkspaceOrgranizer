using Prism.Commands;

namespace RFM.Dialogs.Base
{
    public interface IDialogViewModel<T>
    {
        int? AutoHideIntervalSecs { get; set; }
        IDialogView Dialog { get; set; }
        T DialogResult { get; set; }
        void CloseDialog(T result);
        void StartTimer();
        void StopTimer();

        bool IsLightDismissible { get; set; }
        DelegateCommand LightDismissCommand { get; }
    }
}
