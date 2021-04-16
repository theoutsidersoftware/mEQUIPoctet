using mEQUIPoctet.Source.Config;
using System;
using System.Globalization;
using System.Windows.Data;

namespace mEQUIPoctet.Source.UI.Converter
{
    /// <summary>
    /// Converts a soulgem to its name.
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    class SoulgemNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string soulgemId = value.ToString();

            if (string.IsNullOrWhiteSpace(soulgemId) || soulgemId == "0")
            {
                return "Empty";
            }

            if (Presets.Soulgem.ContainsKey(soulgemId))
            {
                if (Presets.Soulgem[soulgemId].Length > 0)
                {
                    return Presets.Soulgem[soulgemId][0];
                }
            }

            return $"#{soulgemId}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
