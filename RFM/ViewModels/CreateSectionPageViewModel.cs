using Prism.Commands;
using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Dialogs;
using RFM.Models;

namespace RFM.ViewModels
{
    public class CreateSectionPageViewModel : ViewModelBase
    {
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand CreateSectionCommand { get; private set; }

        public CreateSectionPageViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService) : base(workflow, regionManager, dialogService)
        {
            BackCommand = new DelegateCommand(DoGoBack);
            CreateSectionCommand = new DelegateCommand(DoCreateSection, CanCreateSection);
        }

        private void DoGoBack()
        {
            Browse(Pages.Dashboard);
        }

        private void DoCreateSection()
        {
            Workspace newSection = new Workspace
            {
                Name = "Insights",
                Description = "The section for insights projects"
            };
            Workflow.Sections.Add(newSection);
            Browse(Pages.Dashboard);
        }

        private bool CanCreateSection()
        {
            return true;
        }

        protected override void Activate()
        {
        }

        protected override void Deactivate()
        {
        }
    }
}
