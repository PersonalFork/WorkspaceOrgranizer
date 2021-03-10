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

        public NoteItem()
        {
            ItemType = new NoteItemType();
        }

        public override void Browse()
        {
            // No implementation.
        }

        public override void Run(string startupArgs)
        {
            // No implementation.
        }
    }
}
