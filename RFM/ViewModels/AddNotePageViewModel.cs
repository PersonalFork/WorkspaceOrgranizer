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
    public class AddNotePageViewModel : ViewModelBase
    {
        private readonly IPersistenceService _persistenceService;
        private NoteItem _noteItem;

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

        private string _content;
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private string _updateMode;
        public string UpdateMode
        {
            get => _updateMode;
            set => SetProperty(ref _updateMode, value);
        }

        public DelegateCommand CreateNoteCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }

        public AddNotePageViewModel(IWorkflow workflow, IRegionManager regionManager, IDialogService dialogService, IPersistenceService persistenceService) : base(workflow, regionManager, dialogService)
        {
            _persistenceService = persistenceService;
            BackCommand = new DelegateCommand(DoGoBack);
            CreateNoteCommand = new DelegateCommand(DoCreateNote, CanCreateNote)
                .ObservesProperty(() => Name)
                .ObservesProperty(() => Description)
                .ObservesProperty(() => Content);
        }

        private void DoGoBack()
        {
            Browse(Pages.ViewSection);
        }

        private bool CanCreateNote()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description))
            {
                return false;
            }

            if (_noteItem != null)
            {
                return !string.Equals(_noteItem.Name, Name, System.StringComparison.OrdinalIgnoreCase)
                    || !string.Equals(_noteItem.Description, Description, System.StringComparison.OrdinalIgnoreCase)
                    || !string.Equals(_noteItem.Content, Content, System.StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }

        private void DoCreateNote()
        {
            string title = "Success";
            string message = "Note has been updated successfully";
            if (_noteItem != null)
            {
                _noteItem.Name = Name;
                _noteItem.Description = Description;
                _noteItem.Content = Content;
            }
            else
            {
                message = "Note has been created successfully";
                NoteItem item = new NoteItem()
                {
                    Name = Name,
                    Description = Description,
                    Content = Content
                };
                Workflow.SelectedSection.Items.Add(item);
                _noteItem = item;
                CreateNoteCommand?.RaiseCanExecuteChanged();
            }
            Task.Run(() =>
            {
                _persistenceService.SaveOrUpdateWorkflow(Workflow);
            });
            InfoDialogViewModel vm = new InfoDialogViewModel(title, message, Dialogs.Common.AlertType.Success);
            DialogService.ShowDialog(vm, 3);
            Browse(Pages.ViewSection);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            NavigationParameters parameters = navigationContext.Parameters;
            if (parameters.TryGetData(out _noteItem))
            {
                UpdateMode = "Update";
                Name = _noteItem.Name;
                Content = _noteItem.Content;
                Description = _noteItem.Description;
            }
            else
            {
                Name = null;
                Content = null;
                Description = null;
                UpdateMode = "Create";
            }
            base.OnNavigatedTo(navigationContext);
        }

        protected override void Activate()
        {
        }

        protected override void Deactivate()
        {
            _noteItem = null;
        }
    }
}
