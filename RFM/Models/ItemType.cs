using Prism.Mvvm;

namespace RFM.Models
{
    public abstract class ItemType : BindableBase
    {
        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _type;
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private string _filter;
        public string Filter
        {
            get { return _filter; }
            set { SetProperty(ref _filter, value); }
        }

        public abstract void Run(Item application, params string[] args);
        public abstract void RunAsAdmin(Item application, params string[] args);
        public abstract void Browse(Item application);
        public abstract void Open(Item application);

        public ItemType(string type)
        {
            Type = type;
        }
    }
}
