using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Dialogs;
using RFM.Models;
using RFM.Services;

namespace RFM.ViewModels
{
    public class CreateSectionPageViewModel : ViewModelBase
    {
        private readonly IPersistenceService _persistanceService;
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

        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand CreateSectionCommand { get; private set; }

        #endregion

        #region Constructors.

        public CreateSectionPageViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService, IPersistenceService persistanceService) : base(workflow, regionManager, dialogService)
        {
            _persistanceService = persistanceService;
            BackCommand = new DelegateCommand(DoGoBack);
            CreateSectionCommand = new DelegateCommand(DoCreateSection, CanCreateSection)
                .ObservesProperty(() => Name)
                .ObservesProperty(() => Description);
        }

        #endregion

        private void DoGoBack()
        {
            Browse(Pages.Dashboard);
        }

        private void DoCreateSection()
        {
            Workspace newSection = new Workspace
            {
                Name = Name,
                Description = Description
            };
            Workflow.Sections.Add(newSection);
            Workflow.SelectedSection = newSection;
            SaveSettings();
            Browse(Pages.ViewSection);
        }

        private void SaveSettings()
        {
            Task.Run(() =>
            {
                _persistanceService.SaveOrUpdateWorkflow(Workflow);
            });
        }

        private bool CanCreateSection()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);
        }

        protected override void Activate()
        {
        }

        protected override void Deactivate()
        {
            Name = null;
            Description = null;
        }
    }
}
