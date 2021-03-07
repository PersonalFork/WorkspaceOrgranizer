using Prism.Commands;
using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Dialogs;

namespace RFM.ViewModels
{
    public class ViewSectionPageViewModel : ViewModelBase
    {
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand DeleteSectionCommand { get; private set; }
        public DelegateCommand EditSectionCommand { get; private set; }

        public ViewSectionPageViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService) : base(workflow, regionManager, dialogService)
        {
            BackCommand = new DelegateCommand(DoGoBack);
            DeleteSectionCommand = new DelegateCommand(DoDeleteSection);
            EditSectionCommand = new DelegateCommand(DoEditSection);
        }

        private void DoEditSection()
        {
            Browse(Pages.EditSection);
        }

        private void DoDeleteSection()
        {
            string title = "Confirm Delete";
            string question = "Do you want to delete the section ?";
            string yesText = "Yes";
            string noText = "Cancel";
            ConfirmDialogViewModel dialog = new ConfirmDialogViewModel(title, question, yesText, noText);
            bool? dialogResult = DialogService.ShowDialog(dialog);
            if (dialogResult == true)
            {
                Workflow.Sections.Remove(Workflow.SelectedSection);
                Browse(Pages.Dashboard);
            }
        }

        private void DoGoBack()
        {
            Browse(Pages.Dashboard);
        }

        protected override void Activate()
        {
        }

        protected override void Deactivate()
        {
        }
    }
}
