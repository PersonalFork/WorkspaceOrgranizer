using System.Diagnostics;
using System.IO;

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
            if (Directory.Exists(application.Location))
            {
                try
                {
                    Process.Start("explorer.exe", application.Location);
                }
                catch
                {
                    //_logger.Warn("Could not open directory :" + ex.Message);
                }
            }
        }
    }
}
