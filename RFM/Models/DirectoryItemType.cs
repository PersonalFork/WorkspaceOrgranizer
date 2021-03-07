using RFM.Common.Constants;

namespace RFM.Models
{
    public class DirectoryItemType : ItemType
    {
        public DirectoryItemType() : base(ItemTypeConstants.Directory)
        {
            Description = "Any folder/directory.";
        }

        public override void Browse(Item application)
        {
            // TODO: Do the implementation.
        }

        public override void Open(Item application)
        {

        }

        public override void Run(Item application, params string[] args)
        {
        }

        public override void RunAsAdmin(Item application, params string[] args)
        {
        }
    }
}
