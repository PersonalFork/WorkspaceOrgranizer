using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using log4net;

using RFM.Common;
using RFM.Common.Extensions;
using RFM.Models;

namespace RFM.Services
{
    public interface IPersistenceService
    {
        Task<List<Workspace>> GetWorkspaces();
        Task<bool> SaveOrUpdateWorkflow(IWorkflow workflow);
    }

    public class PersistenceService : IPersistenceService
    {
        #region Private Variable Declarations.

        private string _settingsFilePath;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PersistenceService));
        public static readonly object _locker = new object();

        #endregion

        #region Constructors.

        public PersistenceService(string baseDirectory)
        {
            InitializeDirectory(baseDirectory);
        }

        #endregion

        #region Public Method Declarations.

        public async Task<List<Workspace>> GetWorkspaces()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string fileContents = File.ReadAllText(_settingsFilePath);
                    if (fileContents.TryXmlDeserialize<Workflow>(out Workflow workflow))
                    {
                        return workflow.Sections.ToList();
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex);
            }
            return new List<Workspace>();
        }

        public async Task<bool> SaveOrUpdateWorkflow(IWorkflow workflow)
        {
            try
            {
                lock (_locker)
                {
                    string serializedContent = ((Workflow)workflow).ToSerializedXml();
                    if (string.IsNullOrEmpty(serializedContent))
                    {
                        return false;
                    }
                    File.WriteAllText(_settingsFilePath, serializedContent);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex);
            }
            return false;
        }

        #endregion

        #region Private Method Declarations.

        private void InitializeDirectory(string baseDirectory)
        {

            string filePath = Path.Combine(baseDirectory, ".rfm", "setting.rfm");
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            _settingsFilePath = filePath;
        }

        #endregion
    }
}
