using System.Configuration;
using System.Diagnostics;
using System.IO;
using RFM.Common.Constants;

namespace RFM.Models
{
    public class ExecutableItemType : ItemType
    {
        #region Constructors.

        public ExecutableItemType() : base(ItemTypeConstants.Executable)
        {
            Description = "Executable files e.g. .exe, .cmd, .msi, .bat etc.";
            Filter = ConfigurationManager.AppSettings["executableFilter"];
        }

        //public override void Run(Item application, string args)
        //{
        //    Process process = new Process();
        //    string directory = Path.GetDirectoryName(application.Location);
        //    process.StartInfo.WorkingDirectory = directory;

        //    if (string.Equals(Path.GetExtension(application.Location), ".bat", System.StringComparison.OrdinalIgnoreCase))
        //    {
        //        process.StartInfo.UseShellExecute = true;
        //        process.StartInfo.FileName = "cmd.exe";
        //        process.StartInfo.Arguments = $"/c {application.Location} {args}";
        //    }
        //    else
        //    {
        //        process.StartInfo.UseShellExecute = false;
        //        process.StartInfo.FileName = application.Location;
        //        process.StartInfo.Arguments = args;
        //    }

        //    process.StartInfo.CreateNoWindow = false;

        //    process.Start();
        //}

        #endregion
    }
}
