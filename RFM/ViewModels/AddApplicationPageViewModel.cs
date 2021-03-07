using Prism.Commands;
using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Dialogs;

namespace RFM.ViewModels
{
    public class AddApplicationPageViewModel : ViewModelBase
    {
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand AddApplicationCommand { get; private set; }

        public AddApplicationPageViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService) : base(workflow, regionManager, dialogService)
        {
            BackCommand = new DelegateCommand(DoGoBack);
            AddApplicationCommand = new DelegateCommand(DoAddApplication, CanAddApplication);
        }

        private void DoAddApplication()
        {

        }

        private bool CanAddApplication()
        {
            return true;
        }

        private void DoGoBack()
        {
            Browse(Pages.ViewSection);
        }

        protected override void Activate()
        {
        }

        protected override void Deactivate()
        {
        }
    }
}
