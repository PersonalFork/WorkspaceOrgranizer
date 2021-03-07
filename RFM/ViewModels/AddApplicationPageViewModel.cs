using Prism.Commands;
using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Dialogs;
using RFM.Models;

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

        private static int itemId = 1;
        private void DoAddApplication()
        {
            // TODO: Show an application type dialog.
            Item item = new Item
            {
                Name = $"Item {itemId}"
            };
            itemId++;
            Workflow.SelectedSection.Items.Add(item);
            Browse(Pages.ViewSection);
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
