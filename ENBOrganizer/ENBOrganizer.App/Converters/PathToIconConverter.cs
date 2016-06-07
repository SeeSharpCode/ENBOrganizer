using ENBOrganizer.Util;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace ENBOrganizer.App.Converters
{
    public class PathToIconConverter : IValueConverter
    {
        // TODO: return error icon here
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string executableName = value.ToString();

                return Icon.ExtractAssociatedIcon(executableName).ToImageSource();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
