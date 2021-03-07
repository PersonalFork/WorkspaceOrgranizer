using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

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
                if (!File.Exists(filePath))
                {
                    string defaultFilePath = "Assets/Images/DefaultApp.png";
                    if (File.Exists(defaultFilePath))
                    {
                        return new Bitmap(defaultFilePath);
                    }
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
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
