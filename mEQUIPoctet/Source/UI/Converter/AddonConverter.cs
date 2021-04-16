using mEQUIPoctet.Source.Config;
using mEQUIPoctet.Source.Core;
using System;
using System.Globalization;
using System.Windows.Data;

namespace mEQUIPoctet.Source.UI.Converter
{
    /// <summary>
    /// Converts an addon to it's effect text.
    /// </summary>
    [ValueConversion(typeof(Addon), typeof(string))]
    class AddonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Addon addon = value as Addon;

            if (addon == null)
            {
                return "None";
            }

            string addonEffect = $"#{addon.Id}";

            if (Presets.Addon.ContainsKey(addon.Id.ToString()) && Presets.Addon[addon.Id.ToString()].Length > 0)
            {
                addonEffect = Presets.Addon[addon.Id.ToString()][0];
            }

            if (addon.Type == AddonType.Normal)
            {
                return $"{addonEffect} {addon.Value.ToString("+0.##;-#.##")}";
            }

            if (addon.Type == AddonType.UniqueOffensive)
            {
                return $"{addonEffect} ({addon.Value.ToString("0.##")}, {addon.Param2})";
            }

            if (addon.Type == AddonType.UniqueDefensive)
            {
                return $"{addonEffect} ({addon.Value.ToString("0.##")}, {addon.Param2}, {addon.Param3})";
            }

            return $"{addonEffect} ({addon.Value.ToString("0.##")})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
