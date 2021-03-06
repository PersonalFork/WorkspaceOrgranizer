namespace RFM.Dialogs.Base
{
    public interface IDialogView
    {
        object DataContext { get; set; }
        bool? ShowDialog();
        void Close();
    }
}