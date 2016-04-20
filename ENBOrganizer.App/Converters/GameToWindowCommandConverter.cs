using ENBOrganizer.App.ViewModels;
using ENBOrganizer.Model.Entities;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace ENBOrganizer.App.Converters
{
    public class GameToWindowCommandConverter : IValueConverter
    {
        public PathToIconConverter PathToIconConverter { get; set; }
        public GamesViewModel GamesViewModel { get; set; }

        public GameToWindowCommandConverter()
        {
            PathToIconConverter = new PathToIconConverter();
            GamesViewModel = SimpleIoc.Default.GetInstance<GamesViewModel>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // PathToIconConverter pathToIconConverter = new PathToIconConverter();

            return ((IEnumerable<Game>)value).Select(game => new ToggleButton
            {
                Content = new Image
                {
                    Source = (ImageSource)(PathToIconConverter.Convert(game.ExecutablePath, Type.GetType("ImageSource"), null, null)),
                    Width = 24,
                    Height = 24
                },

                IsChecked = game.Equals(GamesViewModel.CurrentGame)
            });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
