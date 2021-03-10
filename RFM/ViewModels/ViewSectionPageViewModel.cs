using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using Prism.Commands;
using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Common.Extensions;
using RFM.Controls.Loader;
using RFM.Dialogs;
using RFM.Models;
using RFM.Services;

namespace RFM.ViewModels
{
    public class ViewSectionPageViewModel : ViewModelBase
    {

        private readonly IPersistenceService _persistanceService;
        private readonly ILoader _loader;
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
        public DelegateCommand ReorderCommand { get; private set; }
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

        public ViewSectionPageViewModel(
            IWorkflow workflow,
            IRegionManager regionManager,
            IDialogService dialogService,
            ILoader loader,
            IPersistenceService persistanceService) : base(workflow, regionManager, dialogService)
        {
            _persistanceService = persistanceService;
            _loader = loader;

            LaunchCommand = new DelegateCommand<Item>(DoLaunch);
            BackCommand = new DelegateCommand(DoGoBack);
            DeleteSectionCommand = new DelegateCommand(DoDeleteSection);
            EditSectionCommand = new DelegateCommand(DoEditSection);
            AddApplicationCommand = new DelegateCommand(DoAddApplication);
            SelectItemCommand = new DelegateCommand<Item>(DoSelectItem);
            OpenInExplorerCommand = new DelegateCommand(DoBrowse);
            DeleteItemCommand = new DelegateCommand(DoDeleteItem);
            EditApplicationCommand = new DelegateCommand<Item>(DoEditApplication);
            ReorderCommand = new DelegateCommand(DoReorder);
            OpenCommand = new DelegateCommand(DoOpenApplication);
            RunCommand = new DelegateCommand(DoRunApplication);
            CloseCommand = new DelegateCommand(DoClose);
            CloneCommand = new DelegateCommand(DoClone);
        }

        private void DoReorder()
        {
            if (SelectedItem == null)
            {
                return;
            }
            int index = Workflow.SelectedSection.Items.IndexOf(SelectedItem);
            if (index == -1)
            {
                return;
            }

            IEnumerable<int> items = Enumerable.Range(1, Workflow.SelectedSection.Items.Count).Where(x => x != index + 1);
            ReorderDialogViewModel vm = new ReorderDialogViewModel(items);
            int? selectedIndex = DialogService.ShowDialog(vm);
            if (selectedIndex != null)
            {
                try
                {
                    _loader.ShowLoader("Please wait while we reorder your items...");
                    Workflow.SelectedSection.Items.Remove(SelectedItem);
                    Workflow.SelectedSection.Items.Insert(selectedIndex.Value, SelectedItem);
                    _persistanceService.SaveOrUpdateWorkflow(Workflow);
                    Workflow.SelectedSection.LastUpdated = DateTime.Now;
                    _loader.HideLoader();
                    string message = $"The item {SelectedItem.Name} has been moved from Position : {index + 1} to Position : {selectedIndex + 1}";
                    InfoDialogViewModel infoDialog = new InfoDialogViewModel("Success", message, Dialogs.Common.AlertType.Success);
                    DialogService.ShowDialog(infoDialog);
                    UnselectItem(SelectedItem);
                    RaisePropertyChanged(nameof(Workflow.Sections));
                }
                catch (System.Exception)
                {
                    _loader.HideLoader();
                    string title = "Error";
                    string message = "Failed to reorder.";
                    InfoDialogViewModel infoDialog = new InfoDialogViewModel(title, message, Dialogs.Common.AlertType.Error);
                    DialogService.ShowDialog(infoDialog, 3);
                }
                finally
                {
                    _loader.HideLoader();
                }
            }
        }

        private void DoClone()
        {
            CloneAppDialogViewModel dialog = new CloneAppDialogViewModel(Workflow.Sections);
            Workspace workspace = DialogService.ShowDialog(dialog);
            if (workspace == null)
            {
                return;
            }

            try
            {
                _loader.ShowLoader("Please wait while we are cloning your selected application..");
                Item clone = SelectedItem.GetClone();
                if (clone == null)
                {
                    return;
                }
                workspace.Items.Add(clone);

                // Save configuration.
                _persistanceService.SaveOrUpdateWorkflow(Workflow);

                // Show clone success dialog.
                _loader.HideLoader();
                string message = $"Item '{SelectedItem.Name}' has been cloned successfully to workspace '{workspace.Name}'";
                InfoDialogViewModel infoDialog = new InfoDialogViewModel("Success", message, Dialogs.Common.AlertType.Success);
                DialogService.ShowDialog(infoDialog, 3);
            }
            finally
            {
                _loader.HideLoader();
            }
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
                    case ItemTypeConstants.Note:
                        Browse(Pages.AddNote, item.ToNavigationParameter());
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
            try
            {
                _loader.ShowLoader("Please wait while we open your selected item...");
                if (SelectedItem is NoteItem)
                {
                    Browse(Pages.AddNote, SelectedItem.ToNavigationParameter());
                }
                else
                {
                    SelectedItem.Open();
                }
            }
            catch (System.Exception)
            {
                _loader.HideLoader();
                string title = "Error";
                string message = "Failed to open application.";
                InfoDialogViewModel vm = new InfoDialogViewModel(title, message, Dialogs.Common.AlertType.Error);
                DialogService.ShowDialog(vm, 3);
            }
            finally
            {
                _loader.HideLoader();
            }
        }

        private void DoRunApplication()
        {
            try
            {
                _loader.ShowLoader("Please wait while we are running your selected item...");
                SelectedItem.ItemType.Run(SelectedItem, SelectedItem.StartupArgs);
            }
            catch (System.Exception)
            {
                _loader.HideLoader();
                string title = "Error";
                string message = "Failed to open.";
                InfoDialogViewModel vm = new InfoDialogViewModel(title, message, Dialogs.Common.AlertType.Error);
                DialogService.ShowDialog(vm, 3);
            }
            finally
            {
                _loader.HideLoader();
            }
        }

        private void DoEditApplication(Item item)
        {
            if (SelectedItem is NoteItem)
            {
                Browse(Pages.AddNote, SelectedItem.ToNavigationParameter());
            }
            else
            {
                Browse(Pages.EditApplication, SelectedItem.ToNavigationParameter());
            }
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
                try
                {
                    _loader.ShowLoader("Please wait while we are deleting your selected item...");

                    Workflow.SelectedSection.Items.Remove(SelectedItem);
                    Workflow.SelectedSection.LastUpdated = DateTime.Now;
                    SelectedItem = null;
                    _persistanceService.SaveOrUpdateWorkflow(Workflow);
                }
                catch (System.Exception)
                {
                    _loader.HideLoader();
                    string errorTitle = "Error";
                    string message = "Failed to delete item.";
                    InfoDialogViewModel vm = new InfoDialogViewModel(errorTitle, message, Dialogs.Common.AlertType.Error);
                    DialogService.ShowDialog(vm, 3);
                }
                finally
                {
                    _loader.HideLoader();
                }
            }
        }

        private void DoBrowse()
        {
            try
            {
                _loader.ShowLoader("Please wait while we open your item...");
                if (SelectedItem == null || string.IsNullOrEmpty(SelectedItem.Location))
                {
                    return;
                }

                SelectedItem.Browse();
            }
            catch (System.Exception)
            {
                _loader.HideLoader();
                string title = "Error";
                string message = "Failed to open.";
                InfoDialogViewModel vm = new InfoDialogViewModel(title, message, Dialogs.Common.AlertType.Error);
                DialogService.ShowDialog(vm, 3);
            }
            finally
            {
                _loader.HideLoader();
            }
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
            if (selectedItemType is NoteItemType)
            {
                Browse(Pages.AddNote);
            }
            else
            {
                Browse(Pages.AddApplication, selectedItemType.ToNavigationParameter());
            }
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
                try
                {
                    _loader.ShowLoader("Please wait while we delete your selected workspace...");
                    Workflow.Sections.Remove(Workflow.SelectedSection);
                    _persistanceService.SaveOrUpdateWorkflow(Workflow);
                    Browse(Pages.Dashboard);
                }
                catch (System.Exception)
                {
                    _loader.HideLoader();
                    string header = "Error";
                    string message = "Failed to delete.";
                    InfoDialogViewModel vm = new InfoDialogViewModel(header, message, Dialogs.Common.AlertType.Error);
                    DialogService.ShowDialog(vm, 3);
                }
                finally
                {
                    _loader.HideLoader();
                }
            }
        }

        private void DoGoBack()
        {
            Browse(Pages.Dashboard);
        }

        protected override void Activate()
        {
            IEnumerable<Item> selectedItems = Workflow?.SelectedSection?.Items?.Where(x => x?.IsSelected == true);
            if (selectedItems != null && selectedItems.Count() > 0)
            {
                foreach (Item item in selectedItems)
                {
                    item.IsSelected = false;
                }
            }
            UnselectItem(SelectedItem);
        }

        protected override void Deactivate()
        {
            UnselectItem(SelectedItem);
        }
    }
}
