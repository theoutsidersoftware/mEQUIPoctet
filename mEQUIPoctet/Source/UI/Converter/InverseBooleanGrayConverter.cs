using System;
using System.Globalization;
using System.Windows.Data;

namespace mEQUIPoctet.Source.UI.Converter
{
    /// <summary>
    /// Converts a boolean to the string "LightGray" if false, or "Black" if true.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(string))]
    class InverseBooleanGrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? wrapper = value as bool?;

            if (!wrapper.HasValue)
            {
                return "LightGray";
            }

            return wrapper.Value ? "Black" : "LightGray";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
