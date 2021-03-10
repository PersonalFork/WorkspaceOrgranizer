using System.Diagnostics;
using RFM.Common.Constants;

namespace RFM.Models
{
    public class HyperlinkItemType : ItemType
    {
        public HyperlinkItemType() : base(ItemTypeConstants.Hyperlink)
        {
            Description = "A link to a web-address e.g http://www.google.com";
        }

        public override void Run(Item application, string args)
        {
            Process process = new Process();
            process.StartInfo.FileName = application.Location;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Arguments = args;
            process.Start();
        }

        public override void Browse(Item application)
        {
            if (!string.IsNullOrEmpty(application.Location))
            {
                Run(application, null);
            }
        }

        public override void Open(Item application)
        {
            Browse(application);
        }
    }
}
