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
    }
}
