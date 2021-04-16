using System;
using System.Globalization;
using System.Windows.Data;

namespace mEQUIPoctet.Source.UI.Converter
{
    /// <summary>
    /// Converts a boolean to the string "LightGray" if true, or "Black" if false.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(string))]
    class BooleanGrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? wrapper = value as bool?;

            if (!wrapper.HasValue)
            {
                return "Black";
            }

            return wrapper.Value ? "LightGray" : "Black";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
