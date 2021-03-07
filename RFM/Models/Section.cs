using System;

using Prism.Mvvm;

namespace RFM.Models
{
    public class Section : BindableBase
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

        public Section()
        {
            Id = Guid.NewGuid().ToString();
            CreatedOn = DateTime.Now;
        }
    }
}
