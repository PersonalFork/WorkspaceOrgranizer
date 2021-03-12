using System.Collections.ObjectModel;
using System.Linq;
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

        private double _fontSize;
        public double FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }

        private string _updateMode;
        public string UpdateMode
        {
            get => _updateMode;
            set => SetProperty(ref _updateMode, value);
        }

        private ObservableCollection<double> _availableFontSizes;
        public ObservableCollection<double> AvailableFontSizes
        {
            get => _availableFontSizes;
            set => SetProperty(ref _availableFontSizes, value);
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
                .ObservesProperty(() => FontSize)
                .ObservesProperty(() => Content);
            AvailableFontSizes = new ObservableCollection<double>(Enumerable.Range(10, 20).Select(x => (double)x));
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
                    || _noteItem.FontSize != FontSize
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
                _noteItem.FontSize = FontSize;
            }
            else
            {
                message = "Note has been created successfully";
                NoteItem item = new NoteItem()
                {
                    Name = Name,
                    Description = Description,
                    Content = Content,
                    FontSize = FontSize
                };
                UpdateMode = "Update";
                Workflow.SelectedSection.Items.Add(item);
                _noteItem = item;
                InfoDialogViewModel vm = new InfoDialogViewModel(title, message, Dialogs.Common.AlertType.Success);
                DialogService.ShowDialog(vm, 3);
            }
            CreateNoteCommand?.RaiseCanExecuteChanged();
            Task.Run(() =>
            {
                _persistenceService.SaveOrUpdateWorkflow(Workflow);
            });

            //Browse(Pages.ViewSection);
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
                FontSize = _noteItem.FontSize;
            }
            else
            {
                Name = null;
                Content = null;
                Description = null;
                UpdateMode = "Create";
                FontSize = new NoteItem().FontSize;
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
