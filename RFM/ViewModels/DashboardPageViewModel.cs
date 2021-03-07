using Prism.Commands;
using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Common.Extensions;
using RFM.Dialogs;
using RFM.Models;

namespace RFM.ViewModels
{
    public class DashboardPageViewModel : ViewModelBase
    {
        public DelegateCommand CreateSectionCommand { get; private set; }
        public DelegateCommand<object> ViewSectionCommand { get; private set; }
        public DashboardPageViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService) : base(workflow, regionManager, dialogService)
        {
            CreateSectionCommand = new DelegateCommand(DoCreateSection);
            ViewSectionCommand = new DelegateCommand<object>(DoViewSection);
        }

        private void DoViewSection(object obj)
        {
            if (!(obj is Section section))
            {
                return;
            }
            Workflow.SelectedSection = section;
            Browse(Pages.ViewSection, section.ToNavigationParameter());
        }

        private void DoCreateSection()
        {
            Browse(Pages.CreateSection);
        }

        protected override void Activate()
        {
            Workflow.SelectedSection = null;
        }

        protected override void Deactivate()
        {
        }
    }
}
