using log4net;

using Prism.Commands;
using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Dialogs;
using RFM.Services;

namespace RFM.ViewModels
{
    public class EditSectionPageViewModel : ViewModelBase
    {
        private readonly IPersistenceService _persistanceService;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(EditSectionPageViewModel));

        public DelegateCommand UpdateSectionCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }

        public EditSectionPageViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService, IPersistenceService persistanceService) : base(workflow, regionManager, dialogService)
        {
            _persistanceService = persistanceService;
            BackCommand = new DelegateCommand(DoGoBack);
            UpdateSectionCommand = new DelegateCommand(DoUpdateSection);
        }

        private void DoUpdateSection()
        {
            string title = "Confirm Update";
            string question = "Do you want to update the section ?";
            string yesText = "Yes";
            string noText = "Cancel";
            ConfirmDialogViewModel dialog = new ConfirmDialogViewModel(title, question, yesText, noText);
            bool? dialogResult = DialogService.ShowDialog(dialog);
            if (dialogResult == true)
            {
                Browse(Pages.Dashboard);
            }
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
