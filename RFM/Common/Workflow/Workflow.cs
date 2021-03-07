using System.Collections.ObjectModel;

using Prism.Mvvm;

using RFM.Models;

namespace RFM.Common
{
    public class Workflow : BindableBase, IWorkflow
    {
        private ObservableCollection<Section> _sections;
        public ObservableCollection<Section> Sections
        {
            get => _sections;
            set => SetProperty(ref _sections, value);
        }

        private Section _selectedSection;
        public Section SelectedSection
        {
            get => _selectedSection;
            set => SetProperty(ref _selectedSection, value);
        }

        public Workflow()
        {
            Sections = new ObservableCollection<Section>();
        }
    }
}
