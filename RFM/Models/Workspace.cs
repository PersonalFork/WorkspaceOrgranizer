using System;
using System.Collections.ObjectModel;

using Prism.Mvvm;

namespace RFM.Models
{
    public class Workspace : BindableBase
    {
        private string _id;
        public string Id
        {
            get => _id;
            private set => SetProperty(ref _id, value);
        }

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

        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public Workspace()
        {
            Id = Guid.NewGuid().ToString();
            CreatedOn = DateTime.Now;
            Items = new ObservableCollection<Item>();
        }
    }
}
