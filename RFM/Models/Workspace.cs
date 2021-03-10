using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

using Prism.Mvvm;

namespace RFM.Models
{
    public class Workspace : BindableBase
    {
        private List<string> _colors;

        private string _id;
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _color;
        public string Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
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

        private DateTime? _lastUpdated;
        public DateTime? LastUpdated
        {
            get => _lastUpdated;
            set => SetProperty(ref _lastUpdated, value);
        }

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get => _createdOn;
            set => SetProperty(ref _createdOn, value);
        }

        private ObservableCollection<Item> _items;
        [XmlArray("Items")]
        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private static Random random;
        public Workspace()
        {
            if (random == null)
            {
                random = new Random();
            }
            PopulateColors();

            Id = Guid.NewGuid().ToString();
            CreatedOn = DateTime.Now;
            Items = new ObservableCollection<Item>();
            Color = _colors[random.Next(0, _colors.Count - 1)];
        }

        private void PopulateColors()
        {
            _colors = new List<string>() {
                "#42D10B",
                "#DE0229",
                "Yellow",
                "Orange",
                "#B300B3",
                "Cyan",
                "#4646E4"
            };
        }
    }
}
