using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mEQUIPoctet.Source.UI
{
    /// <summary>
    /// The accessory specific portion of EquipmentViewModel.
    /// </summary>
    public partial class EquipmentViewModel
    {
        public string PhysicalAttackFlat
        {
            get
            {
                return _equipment.PhysicalAttackFlat.ToString();
            }

            set
            {
                _equipment.PhysicalAttackFlat = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string MagicalAttackFlat
        {
            get
            {
                return _equipment.MagicalAttackFlat.ToString();
            }

            set
            {
                _equipment.MagicalAttackFlat = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string AccessoryPhysicalDefense
        {
            get
            {
                return _equipment.AccessoryPhysicalDefense.ToString();
            }

            set
            {
                _equipment.AccessoryPhysicalDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string AccessoryEvasion
        {
            get
            {
                return _equipment.AccessoryEvasion.ToString();
            }

            set
            {
                _equipment.AccessoryEvasion = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string AccessoryMetalDefense
        {
            get
            {
                return _equipment.AccessoryMetalDefense.ToString();
            }

            set
            {
                _equipment.AccessoryMetalDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string AccessoryWoodDefense
        {
            get
            {
                return _equipment.AccessoryWoodDefense.ToString();
            }

            set
            {
                _equipment.AccessoryWoodDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string AccessoryWaterDefense
        {
            get
            {
                return _equipment.AccessoryWaterDefense.ToString();
            }

            set
            {
                _equipment.AccessoryWaterDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string AccessoryFireDefense
        {
            get
            {
                return _equipment.AccessoryFireDefense.ToString();
            }

            set
            {
                _equipment.AccessoryFireDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }

        public string AccessoryEarthDefense
        {
            get
            {
                return _equipment.AccessoryEarthDefense.ToString();
            }

            set
            {
                _equipment.AccessoryEarthDefense = ParseInt(value);
                NotifyPropertyChanged();
            }
        }
    }
}
