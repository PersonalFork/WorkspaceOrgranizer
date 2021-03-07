using System.Collections.ObjectModel;

using RFM.Models;

namespace RFM.Common
{
    public interface IWorkflow
    {
        ObservableCollection<Workspace> Sections { get; }
        Workspace SelectedSection { get; set; }
    }
}