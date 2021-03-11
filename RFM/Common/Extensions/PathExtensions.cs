using System;
using System.IO;
using Microsoft.Win32;

namespace RFM.Common.Extensions
{
    public static class PathExtensions
    {
        public static bool IsValidPath(this string path)
        {
            try
            {
                Path.GetDirectoryName(path);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public static string SearchDirectory(this string fileName)
        {
            // For rooted paths, return the path as is.
            if (Path.IsPathRooted(fileName))
            {
                if (IsValidPath(fileName))
                {
                    if (Directory.Exists(fileName))
                    {
                        return fileName;
                    }
                    else
                    {
                        return Path.GetDirectoryName(fileName);
                    }
                }
                else
                {
                    return Environment.GetFolderPath(Environment.SpecialFolder.System);
                }
            }

            string path = Environment.GetEnvironmentVariable("path");
            if (string.IsNullOrEmpty(path))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.System);
            }
            string[] paths = path.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (!Path.HasExtension(fileName))
            {
                fileName = $"{fileName}.exe";
            }
            foreach (string dir in paths)
            {
                string appPath = Path.Combine(dir, fileName);
                if (appPath.IsValidPath())
                {
                    if (File.Exists(appPath))
                    {
                        return dir;
                    }
                }
            }
            string registryDirectory = GetRegistryDirectory(fileName);
            if (!string.IsNullOrEmpty(registryDirectory))
            {
                return registryDirectory;
            }

            return Environment.GetFolderPath(Environment.SpecialFolder.System);
        }

        private static string GetRegistryDirectory(string filePath)
        {
            try
            {
                string subKey = $"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\{filePath}";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey))
                {
                    if (key != null)
                    {
                        object o = key.GetValue("");
                        if (o != null)
                        {
                            string path = Convert.ToString(o).Replace("\"", string.Empty);
                            if (IsValidPath(path))
                            {
                                return Path.GetDirectoryName(path);
                            }
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
