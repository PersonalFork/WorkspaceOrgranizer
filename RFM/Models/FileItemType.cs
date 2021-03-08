using System;

using RFM.Common.Constants;

namespace RFM.Models
{
    public class FileItemType : ItemType
    {
        public FileItemType() : base(ItemTypeConstants.File)
        {
            Description = "Any file e.g. text, PDF, image file etc.";
        }
    }
}
