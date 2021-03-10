﻿using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using Prism.Mvvm;

namespace RFM.Models
{
    [XmlInclude(typeof(ExecutableItemType))]
    [XmlInclude(typeof(FileItemType))]
    [XmlInclude(typeof(DirectoryItemType))]
    [XmlInclude(typeof(HyperlinkItemType))]
    [XmlInclude(typeof(NoteItemType))]
    [XmlType("ItemType")]
    public abstract class ItemType : BindableBase
    {
        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _type;
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private string _filter;
        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }

        public virtual void Run(Item application, string args)
        {
            Process process = new Process();
            try
            {
                string directory = Path.GetDirectoryName(application.Location);
                process.StartInfo.WorkingDirectory = directory;
            }
            catch (System.Exception)
            {
                //absorb the exception.  
            }
            process.StartInfo.FileName = application.Location;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Arguments = args;
            process.Start();
        }

        public virtual void RunAsAdmin(Item application, params string[] args)
        {

        }

        public virtual void Browse(Item application)
        {
            string path = Path.GetDirectoryName(application.Location);
            if (Directory.Exists(path))
            {
                try
                {
                    Process.Start("explorer.exe", path);
                }
                catch
                {
                    //_logger.Warn("Could not open directory :" + ex.Message);
                }
            }
        }
        public virtual void Open(Item application)
        {
            Run(application, null);
        }

        public ItemType(string type)
        {
            Type = type;
        }
    }
}
