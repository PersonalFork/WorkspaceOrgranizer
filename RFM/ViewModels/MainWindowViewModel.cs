using System.Threading.Tasks;

using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Common.Extensions;
using RFM.Controls.Loader;
using RFM.Dialogs;

namespace RFM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ILoader Loader { get; private set; }
        public MainWindowViewModel(
            IWorkflow workflow,
            IRegionManager regionManager,
            IDialogService dialogService,
            ILoader loader) : base(workflow, regionManager, dialogService)
        {
            Loader = loader;
            Activate();
        }

        protected override void Activate()
        {
            if (IsActive)
            {
                return;
            }
            Task.Run(() =>
            {
                Loader.ShowLoader("Please wait");
                RegionManager?.BrowseRegionToPage(Regions.HeaderRegion, Pages.HomeHeader);
                RegionManager?.BrowseRegionToPage(Regions.ContentRegion, Pages.Loading);
            });
        }

        protected override void Deactivate()
        {
        }
    }
}
