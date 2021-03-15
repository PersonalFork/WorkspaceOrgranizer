using System;
using RFM.Common.Extensions;

namespace RFM.Models
{
    public class NoteItem : Item
    {
        private string _content;
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private double _fontSize = 16;
        public double FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }

        public NoteItem()
        {
            ItemType = new NoteItemType();
        }

        public override Item GetClone()
        {
            NoteItem item = new NoteItem()
            {
                CreatedOn = DateTime.Now,
                Description = Description,
                Icon = Icon,
                Name = $"{Name}-Clone",
                StartupArgs = StartupArgs,
                Content = Content
            };
            return item;
        }

        public override void Browse()
        {
            // No implementation.
        }

        public override void Run(string startupArgs)
        {
            // No implementation.
        }

        public override bool Contains(string key)
        {
            key = Convert.ToString(key.ToUpper());
            return base.Contains(key) || Content?.Contains(key, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
