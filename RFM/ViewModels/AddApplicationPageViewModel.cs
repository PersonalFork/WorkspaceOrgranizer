using System.Threading.Tasks;
using Prism.Commands;
using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Common.Extensions;
using RFM.Dialogs;
using RFM.Models;
using RFM.Services;

namespace RFM.ViewModels
{
    public class AddApplicationPageViewModel : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _location;
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        private string _commandLineArguments;
        public string CommandLineArguments
        {
            get => _commandLineArguments;
            set => SetProperty(ref _commandLineArguments, value);
        }

        private ItemType _itemType;
        public ItemType ItemType
        {
            get => _itemType;
            set => SetProperty(ref _itemType, value);
        }

        private IPersistenceService _persistanceService;

        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand AddApplicationCommand { get; private set; }

        public AddApplicationPageViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService, IPersistenceService persistanceService) : base(workflow, regionManager, dialogService)
        {
            _persistanceService = persistanceService;
            BackCommand = new DelegateCommand(DoGoBack);
            AddApplicationCommand = new DelegateCommand(DoAddApplication, CanAddApplication);
        }

        private void DoAddApplication()
        {
            Item item = new Item
            {
                Name = Name,
                ItemType = ItemType,
                Location = Location.Replace("'", string.Empty).Replace("\"", string.Empty),
                Description = Description,
                StartupArgs = CommandLineArguments
            };
            Workflow.SelectedSection.Items.Add(item);
            Task.Run(() =>
            {
                _persistanceService.SaveOrUpdateWorkflow(Workflow);
            });
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

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            NavigationParameters parameters = navigationContext?.Parameters;
            if (parameters != null && parameters.TryGetData<ItemType>(out ItemType itemType))
            {
                ItemType = itemType;
            }
            base.OnNavigatedTo(navigationContext);
        }

        protected override void Activate()
        {
        }

        protected override void Deactivate()
        {
            Name = null;
            Description = null;
            CommandLineArguments = null;
            Location = null;
            ItemType = null;
        }
    }
}
