using System;
using System.Xml.Serialization;

using Prism.Mvvm;

using RFM.Common.Extensions;

namespace RFM.Models
{
    [XmlInclude(typeof(NoteItem))]
    [XmlType("Item")]
    public class Item : BindableBase
    {
        #region Public Properties.

        public string Id { get; set; }

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
            set => SetProperty(ref _createdOn, value);
        }

        private DateTime? _lastUpdated;
        public DateTime? LastUpdated
        {
            get => _lastUpdated;
            set => SetProperty(ref _lastUpdated, value);
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

        public virtual Item GetClone()
        {
            Item item = new Item()
            {
                CreatedOn = DateTime.Now,
                Description = Description,
                Icon = Icon,
                ItemType = ItemType,
                Location = Location,
                Name = $"{Name}-Clone",
                StartupArgs = StartupArgs
            };
            return item;
        }

        public virtual bool Contains(string key)
        {
            key = Convert.ToString(key.ToUpper());

            return ItemType?.Type?.Contains(key, StringComparison.OrdinalIgnoreCase) == true
                || Description?.Contains(key, StringComparison.OrdinalIgnoreCase) == true
                || Name?.Contains(key, StringComparison.OrdinalIgnoreCase) == true
                || Location?.Contains(key, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
