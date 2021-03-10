using RFM.Common.Constants;

namespace RFM.Models
{
    public class NoteItemType : ItemType
    {
        public NoteItemType() : base(ItemTypeConstants.Note)
        {
            Description = "Daily notes/memo.";
        }
    }
}
