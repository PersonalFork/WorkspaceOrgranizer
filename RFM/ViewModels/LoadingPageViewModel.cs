using System.Collections.Generic;

using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Dialogs;
using RFM.Services;

namespace RFM.ViewModels
{
    public class LoadingPageViewModel : ViewModelBase
    {
        private IPersistenceService _persistanceService;

        public LoadingPageViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService, IPersistenceService persistanceService) : base(workflow, regionManager, dialogService)
        {
            _persistanceService = persistanceService;
        }

        protected override async void Activate()
        {
            List<Models.Workspace> workspaces = await _persistanceService.GetWorkspaces();
            foreach (Models.Workspace workspace in workspaces)
            {
                Workflow.Sections.Add(workspace);
            }
            Browse(Pages.Dashboard);
        }

        protected override void Deactivate()
        {
        }
    }
}
