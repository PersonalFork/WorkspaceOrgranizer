using RFM.Common.Constants;

namespace RFM.Models
{
    public class DirectoryItemType : ItemType
    {
        public DirectoryItemType() : base(ItemTypeConstants.Directory)
        {
            Description = "Any folder/directory.";
        }
    }
}
