using System.Threading.Tasks;
using Prism.Commands;
using Prism.Regions;
using RFM.Common;
using RFM.Dialogs;

namespace RFM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DelegateCommand DialogCommand { get; private set; }

        public MainWindowViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService) : base(workflow, regionManager, dialogService)
        {
            DialogCommand = new DelegateCommand(DoShowDialog);
            Activate();
        }

        private void DoShowDialog()
        {
            DialogService ds = new DialogService();
            ConfirmDialogViewModel confirmDialogViewModel = new ConfirmDialogViewModel("Test", "Do you want to test ?", cancelText: "Cancel");
            ds.ShowDialog(confirmDialogViewModel);
        }

        protected override void Activate()
        {
            if (IsActive)
            {
                return;
            }
            Task.Run(() =>
            {
                GoHome();
            });
        }

        protected override void Deactivate()
        {
        }
    }
}
