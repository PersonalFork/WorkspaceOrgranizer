using System;

using RFM.Common.Constants;

namespace RFM.Models
{
    public class FileItemType : ItemType
    {
        public FileItemType() : base(ItemTypeConstants.File)
        {
            Description = "Any file e.g. text,PDF,image file etc.";
        }

        public override void Browse(Item application)
        {
            throw new NotImplementedException();
        }

        public override void Open(Item application)
        {
            throw new NotImplementedException();
        }

        public override void Run(Item application, params string[] args)
        {
            throw new NotImplementedException();
        }

        public override void RunAsAdmin(Item application, params string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
