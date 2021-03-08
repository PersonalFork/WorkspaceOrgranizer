using System;

using Prism.Mvvm;

namespace RFM.Models
{
    public class Item : BindableBase
    {
        #region Public Properties.

        public string Id { get; private set; }

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

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get => _createdOn;
            private set => SetProperty(ref _createdOn, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private string _location;
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        private string _startupArgs;
        public string StartupArgs
        {
            get => _startupArgs;
            set => SetProperty(ref _startupArgs, value);
        }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        private ItemType _itemType;
        public ItemType ItemType
        {
            get => _itemType;
            set => SetProperty(ref _itemType, value);
        }

        #endregion

        #region Constructors.

        public Item()
        {
            Id = Guid.NewGuid().ToString();
            CreatedOn = DateTime.Now;
        }

        #endregion

        public virtual void Browse()
        {
            ItemType?.Browse(this);
        }

        public virtual void Run(string startupArgs)
        {
            ItemType?.Run(this, startupArgs);
        }

        public virtual void Open()
        {
            Run(null);
        }
    }
}
