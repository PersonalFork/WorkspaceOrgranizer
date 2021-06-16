using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

using RFM.Common.Extensions;

namespace RFM.Converters
{
    public class FileToImageIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                BitmapSource icon = null;
                if (value == null)
                {
                    return DependencyProperty.UnsetValue;
                }
                string filePath = value.ToString();
                if (Path.IsPathRooted(filePath) == false)
                {
                    string systemDirectoryPath = filePath.SearchDirectory();
                    filePath = Path.Combine(systemDirectoryPath, filePath);
                    if (Path.HasExtension(filePath) == false)
                    {
                        filePath = $"{filePath}.exe";
                    }
                }
                if (!File.Exists(filePath))
                {
                    // TODO: Return a default image.

                    //if (File.Exists(defaultFilePath))
                    //{
                    //    BitmapImage bitmap = new BitmapImage();
                    //    bitmap.BeginInit();
                    //    bitmap.UriSource = new Uri(defaultFilePath, UriKind.Relative);
                    //    bitmap.EndInit();

                    //    icon = bitmap;
                    //    return icon;
                    //}
                    return DependencyProperty.UnsetValue;
                }
                else
                {
                    using (Icon sysicon = Icon.ExtractAssociatedIcon(filePath))
                    {
                        icon = Imaging.CreateBitmapSourceFromHIcon(sysicon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }
                }
                return icon;
            }
            catch (Exception)
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
