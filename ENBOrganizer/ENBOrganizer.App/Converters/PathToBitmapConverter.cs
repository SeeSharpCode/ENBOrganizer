using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ENBOrganizer.App.Converters
{
    public class PathToBitmapConverter : IValueConverter
    {
        // TODO: ensure it's a valid image
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string imagePath = value?.ToString();

                if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath))
                    return App.Current.FindResource("AccentColorBrush");
                
                return new ImageBrush(new BitmapImage(new Uri(imagePath)) { CacheOption = BitmapCacheOption.OnLoad }) { Stretch = Stretch.UniformToFill };
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
