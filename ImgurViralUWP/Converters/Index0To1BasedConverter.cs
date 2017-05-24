using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Data;

namespace ImgurViralUWP.Converters
{
    /// <summary>
    /// Convert 0-based SelectedIndex property to 1-based.
    /// </summary>
    public class Index0To1BasedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && !value.ToString().Equals("-1"))
            {
                int output;
                bool isConverted = int.TryParse(value.ToString(), out output);
                if (isConverted)
                {
                    return (output + 1);
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // ONE-WAY Conversion!
            return null;
        }
    }
}
