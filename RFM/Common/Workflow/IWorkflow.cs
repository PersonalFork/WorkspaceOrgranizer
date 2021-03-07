using System.Collections.ObjectModel;

using RFM.Models;

namespace RFM.Common
{
    public interface IWorkflow
    {
        ObservableCollection<Section> Sections { get; }
        Section SelectedSection { get; set; }
    }
}