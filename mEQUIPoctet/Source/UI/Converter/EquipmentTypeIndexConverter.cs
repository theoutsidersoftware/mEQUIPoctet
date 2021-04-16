using mEQUIPoctet.Source.Core;
using System;
using System.Globalization;
using System.Windows.Data;

namespace mEQUIPoctet.Source.UI.Converter
{
    /// <summary>
    /// Converts Equipment Type to its corresponding TabControl TabItem index.
    /// </summary>
    [ValueConversion(typeof(EquipmentType), typeof(int))]
    class EquipmentTypeIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EquipmentType? wrapper = value as EquipmentType?;

            if (!wrapper.HasValue)
            {
                return 0;
            }

            EquipmentType type = wrapper.Value;

            switch (type)
            {
                case EquipmentType.Weapon:
                    return 0;

                case EquipmentType.Armor:
                    return 1;

                case EquipmentType.Accessory:
                    return 2;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index;
            int? wrapper = value as int?;

            if (wrapper.HasValue)
            {
                index = wrapper.Value;
            }
            else
            {
                if (!int.TryParse(value.ToString(), out index))
                {
                    return EquipmentType.Weapon;
                }
            }

            switch (index)
            {
                case 0:
                    return EquipmentType.Weapon;

                case 1:
                    return EquipmentType.Armor;

                case 2:
                    return EquipmentType.Accessory;
            }

            return EquipmentType.Weapon;
        }
    }
}
