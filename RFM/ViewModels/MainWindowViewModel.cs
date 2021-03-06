using Prism.Commands;
using Prism.Mvvm;
using RFM.Dialogs;

namespace RFM.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DelegateCommand DialogCommand { get; private set; }

        public MainWindowViewModel()
        {
            DialogCommand = new DelegateCommand(DoShowDialog);
        }

        private void DoShowDialog()
        {
            DialogService ds = new DialogService();
            ConfirmDialogViewModel confirmDialogViewModel = new ConfirmDialogViewModel("Test", "Do you want to test ?");
            ds.ShowDialog(confirmDialogViewModel);
        }
    }
}
