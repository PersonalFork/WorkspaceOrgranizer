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
    public class EditApplicationPageViewModel : ViewModelBase
    {
        #region Private Variable Declarations.

        private readonly IPersistenceService _persistenceService;
        private Item _item;

        #endregion

        #region Public Properties.

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

        public DelegateCommand UpdateApplicationCommand { get; }
        public DelegateCommand BackCommand { get; }

        #endregion

        #region Constructors.

        public EditApplicationPageViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService, IPersistenceService persistenceService) : base(workflow, regionManager, dialogService)
        {
            _persistenceService = persistenceService;
            UpdateApplicationCommand = new DelegateCommand(DoUpdateApplication, CanUpdateApplication);
            BackCommand = new DelegateCommand(DoGoBack);
        }

        #endregion

        #region Public/Protected Method Declarations.

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            NavigationParameters navigationParams = navigationContext?.Parameters;
            if (navigationParams != null && navigationParams.TryGetData(out Item item))
            {
                _item = item;
                Name = _item.Name;
                Description = _item.Description;
                Location = _item.Location;
                CommandLineArguments = _item.StartupArgs;
                ItemType = _item.ItemType;
            }
            base.OnNavigatedTo(navigationContext);
        }

        protected override void Activate()
        {
        }

        protected override void Deactivate()
        {
        }

        #endregion

        #region Private Method Declarations.

        private void DoGoBack()
        {
            Browse(Pages.ViewSection);
        }

        private void DoUpdateApplication()
        {
            string title = "Confirm Update";
            string question = $"Do you want to update the item '{_item.Name}'?";
            string yesText = "Yes";
            string noText = "Cancel";
            ConfirmDialogViewModel dialog = new ConfirmDialogViewModel(title, question, yesText, noText);
            bool? dialogResult = DialogService.ShowDialog(dialog);
            if (dialogResult == true)
            {
                _item.Name = Name;
                _item.Description = Description;
                _item.Location = Location.Replace("'", string.Empty).Replace("\"", string.Empty);
                _item.StartupArgs = CommandLineArguments;

                Task.Run(() =>
                {
                    _persistenceService.SaveOrUpdateWorkflow(Workflow);
                });

                Browse(Pages.ViewSection);
            }
        }

        private bool CanUpdateApplication()
        {
            return true;
        }

        #endregion
    }
}
