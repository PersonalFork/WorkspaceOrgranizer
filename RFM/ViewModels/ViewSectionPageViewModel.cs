using System;
using System.Threading.Tasks;
using log4net;
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
    public class ViewSectionPageViewModel : ViewModelBase
    {

        private readonly IPersistenceService _persistanceService;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ViewSectionPageViewModel));

        public DelegateCommand<Item> LaunchCommand { get; }
        public DelegateCommand AddApplicationCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand DeleteSectionCommand { get; private set; }
        public DelegateCommand OpenInExplorerCommand { get; private set; }
        public DelegateCommand EditSectionCommand { get; private set; }
        public DelegateCommand<Item> SelectItemCommand { get; private set; }
        public DelegateCommand DeleteItemCommand { get; private set; }
        public DelegateCommand<Item> EditApplicationCommand { get; private set; }
        public DelegateCommand OpenCommand { get; }
        public DelegateCommand RunCommand { get; }
        public DelegateCommand CloneCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }

        private Item _selectedApplication;
        public Item SelectedItem
        {
            get => _selectedApplication;
            set => SetProperty(ref _selectedApplication, value);
        }

        public ViewSectionPageViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService, IPersistenceService persistanceService) : base(workflow, regionManager, dialogService)
        {
            _persistanceService = persistanceService;
            LaunchCommand = new DelegateCommand<Item>(DoLaunch);
            BackCommand = new DelegateCommand(DoGoBack);
            DeleteSectionCommand = new DelegateCommand(DoDeleteSection);
            EditSectionCommand = new DelegateCommand(DoEditSection);
            AddApplicationCommand = new DelegateCommand(DoAddApplication);
            SelectItemCommand = new DelegateCommand<Item>(DoSelectItem);
            OpenInExplorerCommand = new DelegateCommand(DoBrowse);
            DeleteItemCommand = new DelegateCommand(DoDeleteItem);
            EditApplicationCommand = new DelegateCommand<Item>(DoEditApplication);
            OpenCommand = new DelegateCommand(DoOpenApplication);
            RunCommand = new DelegateCommand(DoRunApplication);
            CloseCommand = new DelegateCommand(DoClose);
            CloneCommand = new DelegateCommand(DoClone);
        }

        private void DoClone()
        {
            CloneAppDialogViewModel dialog = new CloneAppDialogViewModel(Workflow.Sections);
            Workspace workspace = DialogService.ShowDialog(dialog);
            if (workspace == null)
            {
                return;
            }
            Item item = new Item()
            {
                CreatedOn = DateTime.Now,
                Description = SelectedItem.Description,
                Name = $"{SelectedItem.Name}-Clone",
                ItemType = SelectedItem.ItemType,
                Location = SelectedItem.Location,
                StartupArgs = SelectedItem.StartupArgs
            };
            workspace.Items.Add(item);

            // Save configuration.
            Task.Run(() =>
            {
                _persistanceService.SaveOrUpdateWorkflow(Workflow);
            });

            // Show clone success dialog.
            string message = $"Item '{SelectedItem.Name}' has been cloned successfully to workspace '{workspace.Name}'";
            InfoDialogViewModel infoDialog = new InfoDialogViewModel("Success", message, Dialogs.Common.AlertType.Success);
            DialogService.ShowDialog(infoDialog, 3);
        }

        private void DoClose()
        {
            UnselectItem(SelectedItem);
        }

        private void DoLaunch(Item item)
        {
            if (item == null)
            {
                return;
            }
            try
            {
                switch (item.ItemType.Type)
                {
                    case ItemTypeConstants.Directory:
                        item.Browse();
                        break;
                    case ItemTypeConstants.Hyperlink:
                    case ItemTypeConstants.Executable:
                    case ItemTypeConstants.File:
                        item.Run(item.StartupArgs);
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoOpenApplication()
        {
            SelectedItem.Open();
        }

        private void DoRunApplication()
        {
            SelectedItem.ItemType.Run(SelectedItem, SelectedItem.StartupArgs);
        }

        private void DoEditApplication(Item item)
        {
            Browse(Pages.EditApplication, SelectedItem.ToNavigationParameter());
        }

        private void DoDeleteItem()
        {
            string title = "Confirm Delete";
            string question = $"Do you want to delete the item `{SelectedItem.Name}` ?";
            string yesText = "Yes";
            string noText = "No";
            ConfirmDialogViewModel dialog = new ConfirmDialogViewModel(title, question, yesText, noText);
            bool? dialogResult = DialogService.ShowDialog(dialog);
            if (dialogResult == true)
            {
                Workflow.SelectedSection.Items.Remove(SelectedItem);
                SelectedItem = null;
                Task.Run(() =>
                {
                    _persistanceService.SaveOrUpdateWorkflow(Workflow);
                });
            }
        }

        private void DoBrowse()
        {
            if (SelectedItem == null || string.IsNullOrEmpty(SelectedItem.Location))
            {
                return;
            }
            SelectedItem.Browse();
        }

        private void DoSelectItem(Item item)
        {
            if (item == null)
            {
                return;
            }

            if (item == SelectedItem)
            {
                UnselectItem(item);
            }
            else
            {
                if (SelectedItem != null)
                {
                    SelectedItem.IsSelected = false;
                }

                SelectedItem = item;
                SelectedItem.IsSelected = true;
            }
        }

        private void UnselectItem(Item item)
        {
            if (item != null)
            {
                item.IsSelected = false;
            }
            SelectedItem = null;
        }

        private void DoAddApplication()
        {
            ItemTypeDialogViewModel vm = new ItemTypeDialogViewModel();
            ItemType selectedItemType = DialogService.ShowDialog(vm);
            if (selectedItemType == null)
            {
                return;
            }
            Browse(Pages.AddApplication, selectedItemType.ToNavigationParameter());
        }

        private void DoEditSection()
        {
            Browse(Pages.EditSection);
        }

        private void DoDeleteSection()
        {
            string title = "Confirm Delete";
            string question = $"Do you want to delete the workspace : `{Workflow.SelectedSection.Name}` ?";
            string yesText = "Yes";
            string noText = "Cancel";
            ConfirmDialogViewModel dialog = new ConfirmDialogViewModel(title, question, yesText, noText);
            bool? dialogResult = DialogService.ShowDialog(dialog);
            if (dialogResult == true)
            {
                Workflow.Sections.Remove(Workflow.SelectedSection);
                Task.Run(() =>
                {
                    _persistanceService.SaveOrUpdateWorkflow(Workflow);
                });
                Browse(Pages.Dashboard);
            }
        }

        private void DoGoBack()
        {
            Browse(Pages.Dashboard);
        }

        protected override void Activate()
        {
            SelectedItem = null;
        }

        protected override void Deactivate()
        {
            UnselectItem(SelectedItem);
        }
    }
}
