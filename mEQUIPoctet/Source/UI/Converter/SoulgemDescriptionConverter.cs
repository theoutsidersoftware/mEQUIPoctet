using mEQUIPoctet.Source.Config;
using mEQUIPoctet.Source.Core;
using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace mEQUIPoctet.Source.UI.Converter
{
    /// <summary>
    /// Converts a soulgem to its addon's description.
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    class SoulgemDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string soulgem = value.ToString();

            if (string.IsNullOrWhiteSpace(soulgem) || soulgem == "0")
            {
                return "Weapon: None" + Environment.NewLine +
                       "Armor: None" + Environment.NewLine +
                       "Accessory: None";
            }

            if (!Presets.Soulgem.ContainsKey(soulgem))
            {
                return "Weapon: None" + Environment.NewLine +
                       "Armor: None" + Environment.NewLine +
                       "Accessory: None";
            }

            string[] soulgemPreset = Presets.Soulgem[soulgem];

            if (soulgemPreset.Length <= 1)
            {
                return "Weapon: None" + Environment.NewLine +
                       "Armor: None" + Environment.NewLine +
                       "Accessory: None";
            }

            StringBuilder description = new StringBuilder();

            // Weapon addon.
            if (Presets.SoulgemAddon.ContainsKey(soulgemPreset[1]))
            {
                string[] addonPreset = Presets.SoulgemAddon[soulgemPreset[1]];
                description.Append($"Weapon: {ConvertSoulgemAddon(addonPreset)}" + Environment.NewLine);
            }
            else
            {
                description.Append("Weapon: None" + Environment.NewLine);
            }

            // Armor addon.
            if (soulgemPreset.Length > 2 && Presets.SoulgemAddon.ContainsKey(soulgemPreset[2]))
            {
                string[] addonPreset = Presets.SoulgemAddon[soulgemPreset[2]];
                description.Append($"Armor: {ConvertSoulgemAddon(addonPreset)}" + Environment.NewLine);
            }
            else
            {
                description.Append("Armor: None" + Environment.NewLine);
            }

            // Accessory addon.
            if (soulgemPreset.Length > 3 && Presets.SoulgemAddon.ContainsKey(soulgemPreset[3]))
            {
                string[] addonPreset = Presets.SoulgemAddon[soulgemPreset[3]];
                description.Append($"Accessory: {ConvertSoulgemAddon(addonPreset)}");
            }
            else
            {
                description.Append("Accessory: None");
            }

            return description.ToString();
        }

        /// <summary>
        /// Convert a soulgem addon to it's description.
        /// </summary>
        /// <param name="addonPreset">The soulgem addon's preset.</param>
        /// <returns>The description.</returns>
        private string ConvertSoulgemAddon(string[] addonPreset)
        {
            if (addonPreset.Length == 1)
            {
                return $"{addonPreset[0]}";
            }

            if (addonPreset.Length == 2)
            {
                int value;
                if (int.TryParse(addonPreset[1], out value))
                {
                    return $"{addonPreset[0]} {value.ToString("+0;-#")}";
                }             
            }

            if (addonPreset.Length == 3)
            {
                return $"{addonPreset[0]} ({addonPreset[1]}, {addonPreset[2]})";
            }

            if (addonPreset.Length == 4)
            {
                return $"{addonPreset[0]} ({addonPreset[1]}, {addonPreset[2]}, {addonPreset[3]})";
            }

            return $"{addonPreset[0]} ({addonPreset[1]})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
