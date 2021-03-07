using System;
using RFM.Common.Constants;

namespace RFM.Models
{
    public class HyperlinkItemType : ItemType
    {
        public HyperlinkItemType() : base(ItemTypeConstants.Hyperlink)
        {
            Description = "A link to a web-address e.g http://www.google.com";
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
