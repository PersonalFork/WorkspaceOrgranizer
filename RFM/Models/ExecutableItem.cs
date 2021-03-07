using System;
using System.Configuration;

using RFM.Common.Constants;

namespace RFM.Models
{
    public class ExecutableItemType : ItemType
    {
        #region Constructors.

        public ExecutableItemType() : base(ItemTypeConstants.Executable)
        {
            Description = "Executable files e.g. .exe, .cmd, .msi, .bat etc.";
            this.Filter = ConfigurationManager.AppSettings["executableFilter"];
        }

        #endregion

        #region Public Method Declarations.

        public override void Browse(Item application)
        {
            throw new NotImplementedException();
        }

        public override void Open(Item application)
        {
            return;
        }

        public override void Run(Item application, params string[] args)
        {
            throw new NotImplementedException();
        }

        public override void RunAsAdmin(Item application, params string[] args)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
