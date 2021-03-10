using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;

using RFM.Dialogs.Base;

namespace RFM.Dialogs
{
    public class ReorderDialogViewModel : DialogViewModelBase<int?>
    {
        private ObservableCollection<int> _indexes;
        public ObservableCollection<int> Indexes
        {
            get => _indexes;
            set => SetProperty(ref _indexes, value);
        }

        private int? _selectedIndex;
        public int? SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        public DelegateCommand SelectCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public ReorderDialogViewModel(IEnumerable<int> indexes)
        {
            _indexes = new ObservableCollection<int>(indexes);
            SelectedIndex = _indexes.FirstOrDefault();
            SelectCommand = new DelegateCommand(DoSelect, CanSelect).ObservesProperty(() => SelectedIndex);
            CancelCommand = new DelegateCommand(DoCancel);
        }

        private void DoCancel()
        {
            CloseDialog(null);
        }

        private void DoSelect()
        {
            CloseDialog(SelectedIndex - 1);
        }

        private bool CanSelect()
        {
            return SelectedIndex != null;
        }
    }
}
