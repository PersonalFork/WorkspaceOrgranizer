using System;
using System.Threading.Tasks;
using log4net;

using Prism.Commands;
using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Controls.Loader;
using RFM.Dialogs;
using RFM.Services;

namespace RFM.ViewModels
{
    public class EditSectionPageViewModel : ViewModelBase
    {
        private readonly ILoader _loader;
        private readonly IPersistenceService _persistanceService;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(EditSectionPageViewModel));

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

        public DelegateCommand UpdateSectionCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }

        public EditSectionPageViewModel(IWorkflow workflow,
            IRegionManager regionManager,
            ILoader loader,
            IDialogService dialogService,
            IPersistenceService persistanceService) : base(workflow, regionManager, dialogService)
        {
            _loader = loader;
            _persistanceService = persistanceService;
            BackCommand = new DelegateCommand(DoGoBack);
            UpdateSectionCommand = new DelegateCommand(DoUpdateSection, CanUpdateSection)
                .ObservesProperty(() => Name)
                .ObservesProperty(() => Description);
        }

        private bool CanUpdateSection()
        {
            return !string.Equals(Description, Workflow.SelectedSection.Description, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(Name, Workflow.SelectedSection.Name, StringComparison.OrdinalIgnoreCase);
        }

        private void DoUpdateSection()
        {
            string title = "Confirm Update";
            string question = $"Do you want to update the section '{Workflow.SelectedSection.Name}'?";
            string yesText = "Yes";
            string noText = "Cancel";
            ConfirmDialogViewModel dialog = new ConfirmDialogViewModel(title, question, yesText, noText);
            bool? dialogResult = DialogService.ShowDialog(dialog);
            if (dialogResult == true)
            {
                try
                {
                    _loader.ShowLoader("Please wait while workspace is getting updated...");
                    Workflow.SelectedSection.Name = Name;
                    Workflow.SelectedSection.Description = Description;
                    Workflow.SelectedSection.LastUpdated = DateTime.Now;
                    Task.Run(() =>
                    {
                        _persistanceService.SaveOrUpdateWorkflow(Workflow);
                    });

                    Browse(Pages.Dashboard);
                }
                finally
                {
                    _loader.HideLoader();
                }
            }
        }

        private void DoGoBack()
        {
            Browse(Pages.ViewSection);
        }

        protected override void Activate()
        {
            Name = Workflow.SelectedSection.Name;
            Description = Workflow.SelectedSection.Description;
        }

        protected override void Deactivate()
        {
        }
    }
}
