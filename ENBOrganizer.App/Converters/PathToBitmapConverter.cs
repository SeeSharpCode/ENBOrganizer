using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ENBOrganizer.App.Converters
{
    public class PathToBitmapConverter : IValueConverter
    {
        // TODO: this could be more elegant
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string imagePath = value.ToString();

                return new BitmapImage(new Uri(imagePath)) { CacheOption = BitmapCacheOption.OnLoad };
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
