using System.Collections.ObjectModel;

using Prism.Commands;

using RFM.Dialogs.Base;
using RFM.Models;

namespace RFM.Dialogs
{
    public class CloneAppDialogViewModel : DialogViewModelBase<Workspace>
    {
        #region Public Properties.

        private ObservableCollection<Workspace> _workspaces;
        public ObservableCollection<Workspace> Workspaces
        {
            get => _workspaces;
            set => SetProperty(ref _workspaces, value);
        }

        private Workspace _selectedWorkspace;
        public Workspace SelectedWorkspace
        {
            get => _selectedWorkspace;
            set => SetProperty(ref _selectedWorkspace, value);
        }

        public DelegateCommand SelectCommand { get; private set; }
        public DelegateCommand CancelCommand { get; }

        #endregion

        #region Constructors.

        public CloneAppDialogViewModel(ObservableCollection<Workspace> workspaces)
        {
            Workspaces = workspaces;
            SelectCommand = new DelegateCommand(DoSelect, CanSelect).ObservesProperty(() => SelectedWorkspace);
            CancelCommand = new DelegateCommand(DoCancel);
        }

        #endregion

        #region Private Method Declarations.

        private void DoCancel()
        {
            CloseDialog(null);
        }

        private void DoSelect()
        {
            CloseDialog(SelectedWorkspace);
        }

        private bool CanSelect()
        {
            return SelectedWorkspace != null;
        }
    
        #endregion
    }
}
