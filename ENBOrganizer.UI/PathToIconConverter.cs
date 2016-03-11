using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using ENBOrganizer.Util;

namespace ENBOrganizer.UI
{
    public class PathToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string executableName = value.ToString();

            return Icon.ExtractAssociatedIcon(executableName).ToImageSource();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
