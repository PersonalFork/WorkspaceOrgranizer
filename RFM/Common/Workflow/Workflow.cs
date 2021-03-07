using System.Collections.ObjectModel;

using Prism.Mvvm;

using RFM.Models;

namespace RFM.Common
{
    public class Workflow : BindableBase, IWorkflow
    {
        private ObservableCollection<Workspace> _sections;
        public ObservableCollection<Workspace> Sections
        {
            get => _sections;
            set => SetProperty(ref _sections, value);
        }

        private Workspace _selectedSection;
        public Workspace SelectedSection
        {
            get => _selectedSection;
            set => SetProperty(ref _selectedSection, value);
        }

        public Workflow()
        {
            Sections = new ObservableCollection<Workspace>();
        }
    }
}
