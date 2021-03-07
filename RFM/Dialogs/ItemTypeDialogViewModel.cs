using System.Collections.ObjectModel;

using Prism.Commands;

using RFM.Dialogs.Base;
using RFM.Models;

namespace RFM.Dialogs
{
    public class ItemTypeDialogViewModel : DialogViewModelBase<ItemType>
    {
        private ObservableCollection<ItemType> _itemTypes;
        public ObservableCollection<ItemType> ItemTypes
        {
            get => _itemTypes;
            set => SetProperty(ref _itemTypes, value);
        }

        private ItemType _selectedItemType;
        public ItemType SelectedItemType
        {
            get => _selectedItemType;
            set => SetProperty(ref _selectedItemType, value);
        }

        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand SelectCommand { get; private set; }
        public DelegateCommand<ItemType> SelectItemCommand { get; private set; }

        public ItemTypeDialogViewModel()
        {
            CreateItemTypeList();
            SelectItemCommand = new DelegateCommand<ItemType>(DoSelectItem);
            CancelCommand = new DelegateCommand(DoCancel);
            SelectCommand = new DelegateCommand(DoSelect, CanSelect).ObservesProperty(() => SelectedItemType);
        }

        private void DoSelectItem(ItemType obj)
        {
            if (obj == null)
            {
                return;
            }
            if (SelectedItemType != null)
            {
                SelectedItemType.IsSelected = false;
            }
            SelectedItemType = obj;
            SelectedItemType.IsSelected = true;
        }

        private bool CanSelect()
        {
            return SelectedItemType != null;
        }

        private void DoSelect()
        {
            if (SelectedItemType != null)
            {
                SelectedItemType.IsSelected = false;
            }
            CloseDialog(SelectedItemType);
        }

        private void DoCancel()
        {
            if (SelectedItemType != null)
            {
                SelectedItemType.IsSelected = false;
                SelectedItemType = null;
            }
            CloseDialog(null);
        }

        private void CreateItemTypeList()
        {
            ItemTypes = new ObservableCollection<ItemType>();

            ItemType hyperlink = new HyperlinkItemType();
            ItemTypes.Add(hyperlink);

            ItemType executable = new ExecutableItemType();
            ItemTypes.Add(executable);

            ItemType directory = new DirectoryItemType();
            ItemTypes.Add(directory);

            ItemType file = new FileItemType();
            ItemTypes.Add(file);
        }
    }
}
